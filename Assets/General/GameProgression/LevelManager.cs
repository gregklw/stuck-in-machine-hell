using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private LevelSceneGroup _levelSceneGroup;
    [SerializeField] private LoadingBar _loadingBar;
    [SerializeField] private CutsceneManager _cutsceneManager;
    [SerializeField] private SceneField _mainMenuScene;

    private BusEventBinding<LevelProgressEventWrapper> _levelChangeBinding;
    private int _sceneCounter;
    //private List<AsyncOperation> _scenesToProcess = new List<AsyncOperation>();
    public LevelSceneGroup LevelSceneGroup
    {
        get => _levelSceneGroup;
        set => _levelSceneGroup = value;
    }
    private void Awake()
    {
        _levelChangeBinding = new BusEventBinding<LevelProgressEventWrapper>(CommenceLevelProgressChange);
    }

    private void OnEnable()
    {
        EventBus<LevelProgressEventWrapper>.Register(_levelChangeBinding);
    }

    private void OnDisable()
    {
        EventBus<LevelProgressEventWrapper>.Deregister(_levelChangeBinding);
    }

    private void CommenceLevelProgressChange(LevelProgressEventWrapper levelChangeEvent)
    {
        if (_sceneCounter < LevelSceneGroup.LevelScenes.Length)
        {
            AdvanceToNextSectionOfLevel();
            Debug.Log("Load next part!");
        }
        else if (LevelSceneGroup.NextLevel == null)
        {
            StartCoroutine(SetupMainMenuChange());
            Debug.Log("Return to main menu!");
        }
        else
        {
            StartCoroutine(SetupLevelChange());
            Debug.Log("Load next level!");
        }
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
        Scene[] activeScenes = SceneManager.GetAllScenes();

        _loadingBar.ClearRegisteredScenesToProcess();
        _loadingBar.ToggleLoadingBarVisibility(true);
        //Debug.Log("Trigger after GoToMainMenu");

        AsyncOperation menuOperation = SceneManager.LoadSceneAsync(_mainMenuScene, LoadSceneMode.Additive);
        _loadingBar.RegisterSceneOperation(menuOperation);

        while (!menuOperation.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_mainMenuScene));
        for (int i = 0; i < activeScenes.Length; i++)
        {
            _loadingBar.RegisterSceneOperation(SceneManager.UnloadSceneAsync(activeScenes[i]));
        }
        //Debug.Log(_loadingBar);
        while (!_loadingBar.CheckIfScenesFullyProcessed())
        {
            //Debug.Log(_loadingBar.UpdateLoadingBar());
            yield return null;
        }
        //Debug.Log("Finished");
        _loadingBar.ToggleLoadingBarVisibility(false);
    }

    private IEnumerator SetupMainMenuChange()
    {
        yield return StartCoroutine(_cutsceneManager.StartOutroCoroutine());
        _cutsceneManager.AddButtonAction(
            () =>
            {
                GoToMainMenu();
                _cutsceneManager.ClearButtonActions();
            }
        );
    }
    #endregion

    #region LEVEL_TRAVERSAL

    //public IEnumerator StartNewLevel()
    //{
    //    _sceneCounter = 0;
    //    //null reference exception if scenes don't exist
    //    SceneField startScene = LevelSceneGroup.StartScene;
    //    SceneField firstWave = LevelSceneGroup.LevelScenes[_sceneCounter++];

    //    Debug.Log($"Current scene group: {LevelSceneGroup} | Start scene name: {startScene.SceneName} | First wave name: {firstWave.SceneName}");
    //    Debug.Log($"Next scene: {LevelSceneGroup.NextLevel}");

    //    AsyncOperation startSceneOperation = SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);
    //    startSceneOperation.allowSceneActivation = true;
    //    AsyncOperation firstWaveOperation = SceneManager.LoadSceneAsync(firstWave, LoadSceneMode.Additive);
    //    firstWaveOperation.allowSceneActivation = false;

    //    MarkAsActiveSceneAfterLoad(startSceneOperation, startScene);

    //    _loadingBar.RegisterSceneOperation(firstWaveOperation);

    //    yield return StartCoroutine(_loadingBar.ProgressLoadingBar(SetupCutsceneAfterLoadFinish));
    //}

    private IEnumerator SetupLevelChange()
    {
        yield return StartCoroutine(_cutsceneManager.StartOutroCoroutine());
        _cutsceneManager.AddButtonAction(
            () =>
            {
                _cutsceneManager.ClearButtonActions();
                _cutsceneManager.ToggleButtonVisibility(false);
                StartCoroutine(AdvanceToNextLevelCoroutine());
            }
            );

    }
    private void AdvanceToNextSectionOfLevel()
    {
        if (_sceneCounter != 0) SceneManager.UnloadSceneAsync(_levelSceneGroup.LevelScenes[_sceneCounter - 1]);
        SceneField levelPartScene = LevelSceneGroup.LevelScenes[_sceneCounter++];
        _loadingBar.RegisterSceneOperation(SceneManager.LoadSceneAsync(levelPartScene, LoadSceneMode.Additive));
        StartCoroutine(_loadingBar.ProcessScenes(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelPartScene))));
    }

    #endregion

    private void MarkAsActiveSceneAfterLoad(AsyncOperation operation, SceneField scene)
    {
        operation.completed += (asyncOperation) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public IEnumerator AdvanceToNextLevelCoroutine()
    {
        _loadingBar.RegisterSceneOperation(SceneManager.UnloadSceneAsync(LevelSceneGroup.StartScene));
        AsyncOperation currentLevelOperation = SceneManager.UnloadSceneAsync(LevelSceneGroup.LevelScenes[_sceneCounter - 1]);
        LevelSceneGroup = LevelSceneGroup.NextLevel;
        yield return StartCoroutine(StartPlaythroughAtSelectedLevel());
    }



    private void SetupCutsceneAfterLoadFinish()
    {
        _cutsceneManager.TogglePanelVisibility(true);
        _cutsceneManager.ToggleButtonVisibility(true);
        _cutsceneManager.AddButtonAction(
            () =>
            {
                _cutsceneManager.ClearButtonActions();
                _cutsceneManager.ToggleButtonVisibility(false);
                StartCoroutine(EnableWaveStart());
            }

            );
    }

    #region STARTNEWLEVEL

    public IEnumerator StartPlaythroughAtSelectedLevel()
    {
        _sceneCounter = 0;
        SceneField baseScene = LevelSceneGroup.StartScene;
        SceneField firstWave = LevelSceneGroup.LevelScenes[_sceneCounter++];

        //enable loading bar panel
        _loadingBar.ToggleLoadingBarVisibility(true);
        //Debug.Log("Trigger after StartPlaythroughAtSelectedLevel");

        //async load level base and make it the preferred scene when it completes loading
        AsyncOperation baseSceneOperation = SceneManager.LoadSceneAsync(baseScene, LoadSceneMode.Additive);
        _loadingBar.RegisterSceneOperation(baseSceneOperation);
        baseSceneOperation.completed += (async) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseScene));

        //async load 1st wave of level and disable scene activation
        AsyncOperation firstWaveSceneOperation = SceneManager.LoadSceneAsync(firstWave, LoadSceneMode.Additive);
        _loadingBar.RegisterSceneOperation(firstWaveSceneOperation);
        firstWaveSceneOperation.allowSceneActivation = false;

        //disable loading bar panel at 90% and enable standby modal panel
        while (!_loadingBar.CheckIfScenesNinetyProcessed() || !baseSceneOperation.isDone)
        {
            _loadingBar.UpdateLoadingBar();
            yield return null;
        }

        _loadingBar.ToggleLoadingBarVisibility(false);
        _cutsceneManager.TogglePanelVisibility(true);
        _cutsceneManager.ToggleButtonVisibility(true);
        _cutsceneManager.ChangeButtonText("Start Wave");
        _cutsceneManager.AddButtonAction(
        () =>
            {
                _cutsceneManager.ToggleButtonVisibility(false);
                StartCoroutine(EnableWaveStart());
                _cutsceneManager.ClearButtonActions();
            }
         );
    }

    private IEnumerator EnableWaveStart()
    {
        //play intro coroutine
        yield return StartCoroutine(StartWaveCoroutine());

        //activate all scenes on standby once intro is finished
        _loadingBar.ActivateAllProcessedScenes();

        //stall coroutine until all scenes on standby finish loading
        while (!_loadingBar.CheckIfScenesFullyProcessed())
        {
            _loadingBar.UpdateLoadingBar();
            yield return null;
        }

        //clear loaded scenes from list and hide standby modal
        _loadingBar.ClearRegisteredScenesToProcess();
        _cutsceneManager.TogglePanelVisibility(false);
    }
    private IEnumerator StartWaveCoroutine()
    {
        _cutsceneManager.TogglePanelVisibility(true);
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
    }

    #endregion
}
