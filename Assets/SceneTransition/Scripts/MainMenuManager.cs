using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SceneField _persistentGameplay;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    public void StartGame(LevelSceneGroup startingLevelSceneGroup)
    {
        StartCoroutine(StartGameCoroutine(startingLevelSceneGroup));
    }

    private IEnumerator StartGameCoroutine(LevelSceneGroup startingLevelSceneGroup)
    {
        //need to enable loading bar before anything else
        LoadingBar loadingbar = FindObjectOfType<LoadingBar>(true);
        loadingbar.ToggleLoadingBarVisibility(true);
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.LevelSceneGroup = startingLevelSceneGroup;
        Scene menuScene = SceneManager.GetActiveScene();
        AsyncOperation async = SceneManager.LoadSceneAsync(_persistentGameplay, LoadSceneMode.Additive);
        loadingbar.RegisterSceneOperation(async);
        while (!async.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_persistentGameplay));
        AsyncOperation menuUnloadAsync = SceneManager.UnloadSceneAsync(menuScene);
        loadingbar.RegisterSceneOperation(menuUnloadAsync);
        Debug.Log(SceneManager.GetActiveScene().name);
        StartCoroutine(levelManager.StartPlaythroughAtSelectedLevel());
    }
}
