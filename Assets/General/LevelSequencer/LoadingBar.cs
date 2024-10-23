using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    private List<AsyncOperation> _scenesToProcess = new List<AsyncOperation>();

    [SerializeField] private Image _loadingBarImage;
    [SerializeField] private TMP_Text _loadingBarText;
    public void ToggleLoadingBarVisibility(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public float UpdateLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < _scenesToProcess.Count; i++)
        {
            loadProgress += _scenesToProcess[i].progress;
        }
        loadProgress /= _scenesToProcess.Count;

        _loadingBarImage.fillAmount = loadProgress;
        _loadingBarText.text = (Mathf.Round(loadProgress * 1000)/10).ToString() + "%";
        return loadProgress;
    }

    public IEnumerator ProcessScenes(Action onFinishLoading)
    {
        for (int i = 0; i < _scenesToProcess.Count; i++)
        {
            while (!_scenesToProcess[i].isDone)
            {
                yield return null;
            }
        }
        onFinishLoading?.Invoke();
        _scenesToProcess.Clear();
    }

    public IEnumerator ProgressLoadingBar(Action onLoadingCompleteAfterDelay)
    {
        Debug.Log($"Number of scenes to process: {_scenesToProcess.Count}");

        ToggleLoadingBarVisibility(true);

        while (CheckIfScenesNinetyProcessed())
        {
            //UpdateLoadingBar();
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);

        onLoadingCompleteAfterDelay?.Invoke();

        ToggleLoadingBarVisibility(false);
    }

    public void RegisterSceneOperation(AsyncOperation sceneOperationToProcess)
    {
        _scenesToProcess.Add(sceneOperationToProcess);
    }
    public bool CheckIfScenesNinetyProcessed()
    {
        bool assertTrue = true;
        for (int i = 0; i < _scenesToProcess.Count; i++)
        {
            if (_scenesToProcess[i].progress < 0.9f)
            {
                assertTrue = false;
            }
        }
        return assertTrue;
    }

    public bool CheckIfScenesFullyProcessed()
    {
        //Debug.Log(_scenesToProcess.Count);

        bool assertTrue = true;
        for (int i = 0; i < _scenesToProcess.Count; i++)
        {
            if (!_scenesToProcess[i].isDone)
            {
                assertTrue = false;
            }
        }
        return assertTrue;
    }

    public void ActivateAllProcessedScenes()
    {
        _scenesToProcess.ForEach(scene => scene.allowSceneActivation = true);
    }

    public void ClearRegisteredScenesToProcess()
    {
        _scenesToProcess.Clear();
    }
}
