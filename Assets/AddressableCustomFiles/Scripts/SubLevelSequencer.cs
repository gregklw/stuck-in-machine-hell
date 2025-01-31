using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SubLevelSequencer : MonoBehaviour
{
    private CutsceneManager _cutsceneManager;

    private LevelSceneGroup _currentLevelSceneGroup;
    private int _sceneCounter;

    private BusEventBinding<SubLevelProgressionEventWrapper> _levelChangeBinding;
    private BusEventBinding<PlayerDeathEventWrapper> _playerDeathBinding;

    private SceneInstance _baseLevelInstance;
    private SceneInstance _currentSubLevelInstance;

    public SceneInstance BaseLevelInstance
    {
        get => _baseLevelInstance;
        set => _baseLevelInstance = value;
    }

    private void Awake()
    {
        _levelChangeBinding = new BusEventBinding<SubLevelProgressionEventWrapper>(CommenceLevelProgressChange);
        _playerDeathBinding = new BusEventBinding<PlayerDeathEventWrapper>(EndCurrentLevelViaPlayerDeath);
        _cutsceneManager = FindObjectOfType<CutsceneManager>(true);
    }
    private void OnEnable()
    {
        EventBus<SubLevelProgressionEventWrapper>.Register(_levelChangeBinding);
        EventBus<PlayerDeathEventWrapper>.Register(_playerDeathBinding);
    }

    private void OnDisable()
    {
        EventBus<SubLevelProgressionEventWrapper>.Deregister(_levelChangeBinding);
        EventBus<PlayerDeathEventWrapper>.Deregister(_playerDeathBinding);
    }

    private void CommenceLevelProgressChange(SubLevelProgressionEventWrapper levelChangeEvent)
    {
        StartCoroutine(SetupLevelPart(_currentLevelSceneGroup));
    }

    private void EndCurrentLevelViaPlayerDeath(PlayerDeathEventWrapper playerDeathEventWrapper)
    {
        //_cutsceneManager.TogglePanelVisibility(true);
        //_cutsceneManager.ToggleButtonVisibility(true);
        //_cutsceneManager.ChangeButtonText("Return to Main Menu");
        //_cutsceneManager.ChangeHeaderText("Game Over");
        _cutsceneManager.AddButtonAction(
            () =>
            {
                Debug.Log("UNLOAD SCENES WHEN ENDING MENU RAN");
                AsyncOperationHandle<SceneInstance> previousSceneUnloadOp = Addressables.UnloadSceneAsync(_currentSubLevelInstance, false);
                AsyncOperationHandle<SceneInstance> baseSceneUnloadOp = Addressables.UnloadSceneAsync(_baseLevelInstance, false);
                //_cutsceneManager.ClearButtonActions();
                _cutsceneManager.ToggleButtonVisibility(false);
                //GoToMainMenu();
            }

            );
        
    }

    public IEnumerator SetupLevelPart(LevelSceneGroup levelSceneGroup)
    {
        //Start of sublevel
        if (_sceneCounter == 0)
        {
            yield return SetupFirstLevelPart(levelSceneGroup);
        }
        //Last sublevel
        else if (_sceneCounter >= _currentLevelSceneGroup.LevelScenes.Length)
        {

            Debug.Log($"Current Sublevel: {_currentSubLevelInstance.Scene.name} | Current Base Level: {_baseLevelInstance.Scene.name}");

            EventBus<LevelCompleteEventWrapper>.Raise(new LevelCompleteEventWrapper()
            {
                CurrentLevelGroup = _currentLevelSceneGroup,
                BaseSceneInstance = _baseLevelInstance
                //SubLevelSceneInstance = _currentSubLevelInstance
            });
            yield return new WaitForSeconds(1);
            AsyncOperationHandle<SceneInstance> previousSceneUnloadOp = Addressables.UnloadSceneAsync(_currentSubLevelInstance, false);
        }
        //Intermittent sublevel
        else
        {
            yield return new WaitForSeconds(1);
            AsyncOperationHandle<SceneInstance> previousSceneUnloadOp = Addressables.UnloadSceneAsync(_currentSubLevelInstance, false);

            while (!previousSceneUnloadOp.IsDone)
            {
                yield return null;
            }

            yield return SetupNextLevelPart();
            StartCoroutine(ActivateLevelComponents());
        }
    }

    private IEnumerator SetupNextLevelPart()
    {
        Debug.Log("Run Next Level");

        AssetReference currentLevelPart = _currentLevelSceneGroup.LevelScenes[_sceneCounter++];

        //async load 1st wave of level and disable scene activation
        AsyncOperationHandle<SceneInstance> currentLevelSceneOp = Addressables.LoadSceneAsync(currentLevelPart, LoadSceneMode.Additive);
        
        while (!currentLevelSceneOp.IsDone)
        {
            yield return null;
        }

        _currentSubLevelInstance = currentLevelSceneOp.Result;
        Debug.Log($"Handle Result: {currentLevelSceneOp.Result} | Scene Name: {_currentSubLevelInstance.Scene.name}");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentSubLevelInstance.Scene.name));

        List<IAddressableLevelLoadable> levelLoadables = UnityUtils.FindInterfaces<IAddressableLevelLoadable>();

        Debug.Log("loadables count: " + levelLoadables.Count);

        foreach (var levelLoadable in levelLoadables)
        {
            StartCoroutine(levelLoadable.Setup());
        }

        yield return LoadingBar.Instance.DelayUntilOperationsComplete("Setting Up Addressables");
    }

    private IEnumerator ActivateLevelComponents()
    {
        List<IAddressableLevelLoadable> levelLoadables = UnityUtils.FindInterfaces<IAddressableLevelLoadable>();

        foreach (var levelLoadable in levelLoadables)
        {
            yield return levelLoadable.Activate();
        }
    }

    #region FIRSTLEVELPARTSETUPCODE

    private IEnumerator SetupFirstLevelPart(LevelSceneGroup levelSceneGroup)
    {
        _currentLevelSceneGroup = levelSceneGroup;

        yield return SetupNextLevelPart();

        List<IPostAddressableLoadable> postAddressableLoadables = UnityUtils.FindInterfaces<IPostAddressableLoadable>();

        foreach (var postAddressableLoadable in postAddressableLoadables)
        {
            yield return postAddressableLoadable.Init();
        }

        SetupStartButton();
    }

    private void SetupStartButton()
    {
        _cutsceneManager.TogglePanelVisibility(true);
        _cutsceneManager.ToggleButtonVisibility(true);
        _cutsceneManager.ChangeButtonText("Start Wave");
        _cutsceneManager.AddButtonAction(
        () =>
        {
            _cutsceneManager.ToggleButtonVisibility(false);
            StartCoroutine(EnableLevelStart());
            //_cutsceneManager.ClearButtonActions();
        }
         );
    }
    private IEnumerator EnableLevelStart()
    {
        _cutsceneManager.TogglePanelVisibility(true);

        _cutsceneManager.ChangeHeaderText("Loading Wave Assets...");

        //play intro coroutine
        WaitForSeconds delay = new WaitForSeconds(1);
        int numberOfSeconds = 5;

        while (numberOfSeconds > 0)
        {
            _cutsceneManager.ChangeHeaderText($"Wave starting in {numberOfSeconds}");
            numberOfSeconds--;
            yield return delay;
        }
        _cutsceneManager.ChangeHeaderText("");
        _cutsceneManager.TogglePanelVisibility(false);

        StartCoroutine(ActivateLevelComponents());
    }
    #endregion
}
