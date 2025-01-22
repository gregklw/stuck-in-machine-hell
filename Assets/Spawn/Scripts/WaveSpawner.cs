using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WaveSpawner : MonoBehaviour, IAddressableLevelLoadable, IAddressableUnloadable
{
    [SerializeField] private CameraEdgeInstantiationDatum[] _cameraEdgeInstantiationData;
    [SerializeField][Range(0, 20)] private float _spawnTimeInterval;
    [SerializeField][Range(0, 300)] private int _amountToSpawnPerRoutine;
    private Stack<GameObject> _objectPool;
    private CameraEdgeSpawnPoint _spawnPoint;
    private int _totalAmountToSpawn;
    public int TotalAmountToSpawn => _totalAmountToSpawn;

    private List<AsyncOperationHandle<GameObject>> _loadedHandles = new List<AsyncOperationHandle<GameObject>>();

    private void Awake()
    {
        _spawnPoint = FindObjectOfType<CameraEdgeSpawnPoint>();
        _objectPool = new Stack<GameObject>();
        for (int i = 0; i < _cameraEdgeInstantiationData.Length; i++)
        {
            _totalAmountToSpawn += _cameraEdgeInstantiationData[i].AmountToCreate;
        }
    }

    private void OnDisable()
    {
        Unload();
    }

    private void InitSpawner()
    {
        Debug.Log("RUNNING");
        List<GameObject> initialList = new List<GameObject>();
        for (int i = 0; i < _cameraEdgeInstantiationData.Length; i++)
        {
            for (int j = 0; j < _cameraEdgeInstantiationData[i].AmountToCreate; j++)
            {
                GameObject go = UnityUtils.InstantiateDisabled(_cameraEdgeInstantiationData[i].LoadedGameObject);
                _spawnPoint.SetSpawnPoint(go, _cameraEdgeInstantiationData[i].EdgeDistanceMinRange, _cameraEdgeInstantiationData[i].EdgeDistanceMaxRange);
                initialList.Add(go);
            }
        }
        UnityUtils.Shuffle(initialList);
        initialList.ForEach(x => _objectPool.Push(x));
    }

    private void SpawnGameObjects(int amountToSpawn)
    {
        Debug.Log(_objectPool.Count + "/" + amountToSpawn);
        while (_objectPool.Count > 0 && amountToSpawn > 0)
        {
            SpawnGameObject();
            amountToSpawn -= 1;
        }
    }

    private void SpawnGameObject()
    {
        if (_objectPool.Count > 0)
        {
            GameObject go = _objectPool.Pop();
            go.SetActive(true);
        }
    }

    public IEnumerator Setup()
    {
        Debug.Log("Setup Enabled");

        foreach (var cameraEdgeInstantiationDatum in _cameraEdgeInstantiationData)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(cameraEdgeInstantiationDatum.AssetReferencePrefab);
            _loadedHandles.Add(handle);
            LoadingBar.Instance.RegisterOperation(handle);
            yield return handle;
            cameraEdgeInstantiationDatum.LoadedGameObject = handle.Result;
        }
        InitSpawner();
    }

    public IEnumerator Activate()
    {
        int currentAmountRemaining = _totalAmountToSpawn;
        while (currentAmountRemaining > 0)
        {
            SpawnGameObjects(_amountToSpawnPerRoutine);
            currentAmountRemaining -= _amountToSpawnPerRoutine;
            yield return Helpers.GetWaitForSeconds(_spawnTimeInterval);
        }
    }

    public void Unload()
    {
        foreach (var loadedHandle in _loadedHandles)
        {
            Debug.Log($"Unloading: {loadedHandle.Result}");
            Addressables.Release(loadedHandle);
        }
    }
}
