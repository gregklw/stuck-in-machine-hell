using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public static LoadingBar Instance { get; private set; }

    //private List<AsyncOperationHandle<SceneInstance>> _scenesToProcess = new List<AsyncOperationHandle<SceneInstance>>();
    private List<AsyncOperationHandle> _operationsToProcess = new List<AsyncOperationHandle>();

    [SerializeField] private Image _loadingBarImage;
    [SerializeField] private TMP_Text _loadingBarText;

    private void Awake()
    {
        Instance ??= this;
        Debug.Log(Instance);
    }

    public void ToggleLoadingBarVisibility(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    #region LOADING SCENE PROGRESS
    //load operations
    public IEnumerator ProcessOperations(string nameOfLoadingOperation, Action callbackDuringProcess = null)
    {
        bool assertAllLoaded;
        do
        {
            int operationsCount = _operationsToProcess.Count;
            if (operationsCount == 0) yield break;

            assertAllLoaded = true;
            float totalProgress = 0;
            for (int i = 0; i < operationsCount; i++)
            {
                var currentOperation = _operationsToProcess[i];
                if (!currentOperation.IsDone)
                { 
                    assertAllLoaded = false;
                }
                if (currentOperation.IsValid()) totalProgress += currentOperation.PercentComplete;
            }

            totalProgress /= operationsCount;
            _loadingBarText.text = $"Operation: {nameOfLoadingOperation} ";
            _loadingBarImage.fillAmount = totalProgress;
            _loadingBarText.text += (Mathf.Round(totalProgress * 1000) / 10).ToString() + "%";

            callbackDuringProcess?.Invoke();
            yield return null;
        }
        while (!assertAllLoaded);
        
        _operationsToProcess.Clear();
    }

    public void RegisterHandleOperation(AsyncOperationHandle operationToProcess)
    {
        _operationsToProcess.Add(operationToProcess);
    }
    //public bool CheckIfOperationsNinetyProcessed()
    //{
    //    bool assertTrue = true;
    //    for (int i = 0; i < _operationsToProcess.Count; i++)
    //    {
    //        if (_operationsToProcess[i].PercentComplete < 0.9f)
    //        {
    //            assertTrue = false;
    //        }
    //    }
    //    return assertTrue;
    //}

    //public bool CheckIfOperationsFullyProcessed()
    //{
    //    bool assertTrue = true;
    //    for (int i = 0; i < _operationsToProcess.Count; i++)
    //    {
    //        //Debug.Log(operationsToProcess[i].IsDone + "/" + operationsToProcess.Count);
    //        if (!_operationsToProcess[i].IsDone)
    //        {
    //            assertTrue = false;
    //        }
    //    }
    //    return assertTrue;
    //}

    //public void ActivateAllProcessedScenes(List<AsyncOperationHandle> operationsToProcess)
    //{
    //    _scenesToProcess.ForEach(scene => scene.Result.ActivateAsync());
    //}

    //public void ClearRegisteredScenesToProcess()
    //{
    //    _scenesToProcess.Clear();
    //}
    #endregion

    //#region LOADING ASSET PROGRESS
    //public IEnumerator ProcessAssets()
    //{
    //    for (int i = 0; i < _assetsToProcess.Count; i++)
    //    {
    //        while (!_assetsToProcess[i].IsDone)
    //        {
    //            yield return null;
    //        }
    //    }
    //}

    //public bool CheckIfAssetsFullyProcessed()
    //{
    //    bool assertTrue = true;
    //    for (int i = 0; i < _assetsToProcess.Count; i++)
    //    {
    //        if (!_assetsToProcess[i].IsDone)
    //        {
    //            assertTrue = false;
    //        }
    //    }
    //    return assertTrue;
    //}

    //public float UpdateAssetLoadingProgress()
    //{
    //    float loadProgress = 0f;
    //    Debug.Log(_assetsToProcess.Count);
    //    _assetsToProcess.ForEach(i =>
    //    {
    //        Debug.Log(i.DebugName + " " + i.PercentComplete);
    //        loadProgress += i.PercentComplete;
    //    }
    //    );
    //    loadProgress /= _assetsToProcess.Count;
    //    _loadingBarText.text = "Asset Loading: ";
    //    UpdateLoadingBar(loadProgress);
    //    return loadProgress;
    //}

    //public void RegisterAssetLoadOperation(AsyncOperationHandle handleToProcess)
    //{
    //    _assetsToProcess.Add(handleToProcess);
    //}
    //#endregion
}
