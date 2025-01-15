using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CutsceneManager _cutsceneManager;
    [SerializeField] private AssetReference _mainMenuScene;
    [SerializeField] private AssetReference _persistentGameplay;
    [SerializeField] private AssetLabelReference _mainMenuLabelReference;
    [SerializeField] private LevelSceneGroup _currentLevelSceneGroup;

    private BusEventBinding<LevelCompleteEventWrapper> _levelChangeBinding;
    private BusEventBinding<PlayerDeathEventWrapper> _playerDeathBinding;
    private Action<LevelCompleteEventWrapper> _levelChangeAction;

    private int _sceneCounter;
    private SceneInstance _currentFocusSceneInstance;
    private SceneInstance _persistentGameplaySceneInstance;

    private LoadingBar _loadingBar;

    private SceneInstance CurrentFocusSceneInstance
    {
        get
        {
            Debug.Log($"Current active scene name: {_currentFocusSceneInstance.Scene.name}");
            return _currentFocusSceneInstance;
        }
        set
        {
            string previousSceneInstanceName = _currentFocusSceneInstance.Scene.name;
            _currentFocusSceneInstance = value;
            Debug.Log($"Changing from {previousSceneInstanceName} to {value.Scene.name}");
        }
    }

    private bool _loadedMainMenuBefore = false;

    private const string InitializationSceneName = "Initialization";

    public static bool DidMousePressUI;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _loadingBar = LoadingBar.Instance;
        _levelChangeAction = (levelCompleteEventWrapper) => StartCoroutine(SetupEndOfLevelChange(levelCompleteEventWrapper));
        _levelChangeBinding = new BusEventBinding<LevelCompleteEventWrapper>(_levelChangeAction);
        _playerDeathBinding = new BusEventBinding<PlayerDeathEventWrapper>(SetupGameOverSequence);
    }

    private void OnEnable()
    {
        EventBus<LevelCompleteEventWrapper>.Register(_levelChangeBinding);
        EventBus<PlayerDeathEventWrapper>.Register(_playerDeathBinding);
    }

    private void OnDisable()
    {
        EventBus<LevelCompleteEventWrapper>.Deregister(_levelChangeBinding);
        EventBus<PlayerDeathEventWrapper>.Deregister(_playerDeathBinding);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            PointerEventData eventData = new PointerEventData(_eventSystem);
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventData.position = Input.mousePosition;
            _graphicRaycaster.Raycast(eventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                DidMousePressUI = true;
            }
        }
        else
        {   
            DidMousePressUI = false;
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Caching.ClearCache();
        //}
    }

    #region MENU_TRAVERSAL
    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuCoroutine());
    }

    public IEnumerator GoToMainMenuCoroutine()
    {
        _cutsceneManager.TogglePanelVisibility(false);
        _cutsceneManager.ToggleButtonVisibility(false);

        _loadingBar.ToggleLoadingBarVisibility(true);

        //yield return DownloadGameData(_mainMenuLabelReference.labelString);

        //yield return new WaitForSeconds(1);

        yield return CheckDownloadStatus(_mainMenuLabelReference.labelString);

        AsyncOperationHandle<SceneInstance> menuOperation = Addressables.LoadSceneAsync(_mainMenuScene, LoadSceneMode.Additive);
        _loadingBar.RegisterHandleOperation(menuOperation);
        yield return _loadingBar.ProcessOperations("Loading Menu");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(menuOperation.Result.Scene.name));

        if (!_loadedMainMenuBefore) SceneManager.UnloadSceneAsync(InitializationSceneName);
        else
        {
            AsyncOperationHandle<SceneInstance> persistentGameplayUnload = Addressables.UnloadSceneAsync(_persistentGameplaySceneInstance, false);
            _loadingBar.RegisterHandleOperation(persistentGameplayUnload);
        }

        //yield return _loadingBar.ProcessOperations("Unloading Unneeded Scenes");

        CurrentFocusSceneInstance = menuOperation.Result;

        LoadAllCurrentLevelAssets();

        yield return _loadingBar.ProcessOperations("Loading Unneeded Scenes and Menu Assets");

        _loadingBar.ToggleLoadingBarVisibility(false);
        if (!_loadedMainMenuBefore) _loadedMainMenuBefore = true;
    }

    
    #endregion

    #region LEVEL_TRAVERSAL


    //need this because main menu button leaves when scene changes thus stopping the coroutine
    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine(_currentLevelSceneGroup));
    }

    private IEnumerator StartGameCoroutine(LevelSceneGroup startingLevelSceneGroup)
    {
        var nextLevelSceneGroup = startingLevelSceneGroup.NextLevel;

        //need to enable loading bar before anything else
        LoadingBar loadingbar = FindObjectOfType<LoadingBar>(true);
        loadingbar.ToggleLoadingBarVisibility(true);

        //yield return DownloadGameData(_persistentGameplay);

        AsyncOperationHandle<SceneInstance> persistentGameplayHandle = Addressables.LoadSceneAsync(_persistentGameplay, LoadSceneMode.Additive);
        _loadingBar.RegisterHandleOperation(persistentGameplayHandle);
        yield return _loadingBar.ProcessOperations("Loading Persistent Gameplay Scene");

        _persistentGameplaySceneInstance = persistentGameplayHandle.Result;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(persistentGameplayHandle.Result.Scene.name));

        //unload menu scene
        AsyncOperationHandle<SceneInstance> unloadMenuHandle = Addressables.UnloadSceneAsync(CurrentFocusSceneInstance, false);
        _loadingBar.RegisterHandleOperation(unloadMenuHandle);
        yield return _loadingBar.ProcessOperations("Unloading Menu Scene");

        StartCoroutine(StartPlaythroughAtSelectedLevel(startingLevelSceneGroup));
    }
    public IEnumerator AdvanceToNextLevelCoroutine(LevelSceneGroup currentLevelGroup)
    {
        yield return StartCoroutine(StartPlaythroughAtSelectedLevel(currentLevelGroup.NextLevel));
    }

    private IEnumerator SetupEndOfLevelChange(LevelCompleteEventWrapper levelCompleteEventWrapper)
    {
        Debug.Log($"Does next level exist?: {levelCompleteEventWrapper.CurrentLevelGroup.NextLevel}");
        yield return StartCoroutine(_cutsceneManager.StartOutroCoroutine());
        _cutsceneManager.AddButtonAction(
            () =>
            {
                StartCoroutine(EndOfLevelButtonCoroutine(levelCompleteEventWrapper));
                //_cutsceneManager.ClearButtonActions();
            }
        );
    }


    private IEnumerator EndOfLevelButtonCoroutine(LevelCompleteEventWrapper levelCompleteEventWrapper)
    {
        AsyncOperationHandle unloadBaseLevelHandle = Addressables.UnloadSceneAsync(levelCompleteEventWrapper.BaseSceneInstance, false);
        _loadingBar.RegisterHandleOperation(unloadBaseLevelHandle);
        Debug.Log($"Is handle done loading: {unloadBaseLevelHandle.IsDone}");

        yield return _loadingBar.ProcessOperations("Unloading Base Level Scene");

        if (levelCompleteEventWrapper.CurrentLevelGroup.NextLevel == null)
        {
            GoToMainMenu();
        }
        else
        {
            StartCoroutine(AdvanceToNextLevelCoroutine(levelCompleteEventWrapper.CurrentLevelGroup));
        }
    }
    #endregion


    #region GAMEOVER

    private void SetupGameOverSequence(PlayerDeathEventWrapper wrapper)
    {
        _cutsceneManager.TogglePanelVisibility(true);
        _cutsceneManager.ToggleButtonVisibility(true);
        _cutsceneManager.ChangeButtonText("Return to Main Menu");
        _cutsceneManager.ChangeHeaderText("Game Over");
        _cutsceneManager.AddButtonAction(
            () =>
            {
                Debug.Log("GAMEOVER RAN");
                //_cutsceneManager.ClearButtonActions();
                _cutsceneManager.ToggleButtonVisibility(false);
                GoToMainMenu();
            }

            );
    }

    #endregion

    #region STARTNEWLEVEL

    public IEnumerator StartPlaythroughAtSelectedLevel(LevelSceneGroup currentSceneGroup)
    {
        _sceneCounter = 0;
        AssetReference baseScene = currentSceneGroup.StartScene;

        //enable loading bar panel
        _loadingBar.ToggleLoadingBarVisibility(true);

        //async load level base and make it the preferred scene when it completes loading
        AsyncOperationHandle<SceneInstance> baseSceneOperation = Addressables.LoadSceneAsync(baseScene, LoadSceneMode.Additive);
        _loadingBar.RegisterHandleOperation(baseSceneOperation);

        baseSceneOperation.Completed += (async) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneOperation.Result.Scene.name));

        yield return _loadingBar.ProcessOperations("Loading Base Scene");

        //_handlesForLoadingOperations.Clear();
        SubLevelSequencer levelSequencer = FindObjectOfType<SubLevelSequencer>();
        levelSequencer.BaseLevelInstance = baseSceneOperation.Result;
        yield return levelSequencer.SetupLevelPart(currentSceneGroup);

        //load starting addressables
        LoadAllCurrentLevelAssets();

        yield return _loadingBar.ProcessOperations("Loading Addressable Scene Assets");

        _loadingBar.ToggleLoadingBarVisibility(false);
    }

    #endregion

    private void LoadAllCurrentLevelAssets()
    {
        List<IAddressableSceneStartLoadable> addressableLoadables = UnityUtils.FindInterfaces<IAddressableSceneStartLoadable>();

        foreach (var loadable in addressableLoadables)
        {
            StartCoroutine(loadable.LoadAddressables());
        }
    }

    #region DOWNLOAD_CODE

    private IEnumerator CheckDownloadStatus(string label)
    {
        //AsyncOperationHandle<IResourceLocator> resourceHandle = default;
        AsyncOperationHandle<long> downloadSizeHandle = default;

        try
        {
            downloadSizeHandle = Addressables.GetDownloadSizeAsync(label);
        }
        catch (Exception ex)
        {
            Debug.Log("Error occured: " + ex.Message);
        }

        yield return downloadSizeHandle;

        if (downloadSizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            float downloadSize = downloadSizeHandle.Result / (1024f * 1024f);
            Debug.Log($"Download Size Currently: {downloadSize}");
            //if (downloadSizeHandle.Result > 0)
            //{
            //}
        }
    }

    //private void InitiateDownload(string label)
    //{
    //    Debug.Log($"Downloading {label}" );
    //    StartCoroutine(DownloadGameData(label));
    //}

    //private IEnumerator DownloadGameData(string label)
    //{
    //    yield return CheckDownloadStatus(label);
    //    //AsyncOperationHandle downloadHandle = default;
    //    //try
    //    //{
    //    //    downloadHandle = Addressables.DownloadDependenciesAsync(label);
    //    //    _loadingBar.RegisterHandleOperation(downloadHandle);
    //    //    downloadHandle.Completed += OnDownloadComplete;
    //    //}
    //    //catch (Exception ex)
    //    //{ 
    //    //    Debug.Log("Error occured: " + ex.Message);
    //    //}
    //    List<string> caches = new List<string>();
    //    Caching.GetAllCachePaths(caches);

    //    foreach (var cache in caches)
    //    {
    //        Debug.Log(cache);
    //    }

    //    Addressables.ClearDependencyCacheAsync(label);

    //    yield return _loadingBar.ProcessOperations("Downloading");
    //}

    //private void OnDownloadComplete(AsyncOperationHandle handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        Addressables.Release(handle);
    //        Debug.Log("Download Completed!");
    //    }

    //    else
    //    {
    //        Addressables.Release(handle);
    //        Debug.Log("Download Failed!");
    //    }
    //}

    #endregion
}
