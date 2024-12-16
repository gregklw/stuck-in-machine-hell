using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    private List<AsyncOperation> _scenesToProcess = new List<AsyncOperation>();
    private List<AsyncOperationHandle> _assetsToProcess = new List<AsyncOperationHandle>();

    [SerializeField] private Image _loadingBarImage;
    [SerializeField] private TMP_Text _loadingBarText;
    public void ToggleLoadingBarVisibility(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    private float UpdateLoadingBar(float loadProgress)
    {
        _loadingBarImage.fillAmount = loadProgress;
        _loadingBarText.text += (Mathf.Round(loadProgress * 1000) / 10).ToString() + "%";
        return loadProgress;
    }

    #region LOADING SCENE PROGRESS

    public float UpdateSceneLoadingProgress()
    {
        float loadProgress = 0f;
        _scenesToProcess.ForEach(i => loadProgress += i.progress);
        loadProgress /= _scenesToProcess.Count;
        _loadingBarText.text = "Scene Loading: ";
        UpdateLoadingBar(loadProgress);
        return loadProgress;
    }

    public IEnumerator ProcessScenes()
    {
        for (int i = 0; i < _scenesToProcess.Count; i++)
        {
            while (!_scenesToProcess[i].isDone)
            {
                yield return null;
            }
        }
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
    #endregion


    #region LOADING ASSET PROGRESS
    public IEnumerator ProcessAssets()
    {
        for (int i = 0; i < _assetsToProcess.Count; i++)
        {
            while (!_assetsToProcess[i].IsDone)
            {
                yield return null;
            }
        }
    }

    public bool CheckIfAssetsFullyProcessed()
    {
        bool assertTrue = true;
        for (int i = 0; i < _assetsToProcess.Count; i++)
        {
            if (!_assetsToProcess[i].IsDone)
            {
                assertTrue = false;
            }
        }
        return assertTrue;
    }

    public float UpdateAssetLoadingProgress()
    {
        float loadProgress = 0f;
        Debug.Log(_assetsToProcess.Count);
        _assetsToProcess.ForEach(i =>
        {
            Debug.Log(i.DebugName + " " + i.PercentComplete);
            loadProgress += i.PercentComplete;
        }
        );
        loadProgress /= _assetsToProcess.Count;
        _loadingBarText.text = "Asset Loading: ";
        UpdateLoadingBar(loadProgress);
        return loadProgress;
    }

    public void RegisterAssetLoadOperation(AsyncOperationHandle handleToProcess)
    {
        _assetsToProcess.Add(handleToProcess);
    }
    #endregion
}
