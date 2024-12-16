using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSpawner : MonoBehaviour, IAddressableLoadable
{
    [SerializeField] private AssetReferenceGameObject _objectToSpawn;
    private GameObject _loadedPrefab;
    public AssetReferenceGameObject ObjectToSpawn
    {
        get => _objectToSpawn;
        set => _objectToSpawn = value;
    }

    //private IEnumerator SpawnAssetReferenceObject()
    //{
    //    AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_objectToSpawn);
    //    yield return handle;
    //    if (handle.Result != null) Instantiate(handle.Result, transform.position, Quaternion.identity);
    //}

#if UNITY_EDITOR
    private GameObject _preview;

    public void SpawnPreview()
    {
        StartCoroutine(SpawnPreviewCoroutine());
    }

    public void DestroyPreview()
    {
        if (_preview != null) DestroyImmediate(_preview);
    }

    private IEnumerator SpawnPreviewCoroutine()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_objectToSpawn);
        yield return handle;
        //_preview ??= Instantiate(handle.Result, transform.position, Quaternion.identity);
        //Debug.Log(_preview);
        if (_preview == null)
        {
            _preview = Instantiate(handle.Result, transform.position, Quaternion.identity);
            _preview.hideFlags = HideFlags.DontSave;
        }

        Debug.Log("EXECUTE: " + _preview);
    }
#endif

    public IEnumerator LoadAddressables(List<AsyncOperationHandle> handles)
    {
        List<AsyncOperationHandle> handleList = new List<AsyncOperationHandle>();
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_objectToSpawn);
        handles.Add(handle);
        yield return handle;
        _loadedPrefab = handle.Result;
        Debug.Log(_loadedPrefab);
    }

    public void Init()
    {
        Debug.Log(_loadedPrefab + " Initialized");
        Instantiate(_loadedPrefab, transform.position, Quaternion.identity);
    }
}
