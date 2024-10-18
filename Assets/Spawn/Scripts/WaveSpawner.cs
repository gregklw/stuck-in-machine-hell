using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private CameraEdgeInstantiationDatum[] _cameraEdgeInstantiationData;
    [SerializeField][Range(0,20)] private float _spawnTimeInterval;
    [SerializeField][Range(0, 300)] private int _amountToSpawnPerRoutine;
    private Stack<GameObject> _objectPool;
    private CameraEdgeSpawnPoint _spawnPoint;
    private int _totalAmountToSpawn;
    public int TotalAmountToSpawn => _totalAmountToSpawn;

    public void InitSpawner()
    {
        _spawnPoint = FindObjectOfType<CameraEdgeSpawnPoint>();
        _objectPool = new Stack<GameObject>();
        List<GameObject> initialList = new List<GameObject>();
        for (int i = 0; i < _cameraEdgeInstantiationData.Length; i++)
        {
            for (int j = 0; j < _cameraEdgeInstantiationData[i].AmountToCreate; j++)
            {
                GameObject go = UnityUtils.InstantiateDisabled(_cameraEdgeInstantiationData[i].Prefab);
                _spawnPoint.SetSpawnPoint(go, _cameraEdgeInstantiationData[i].EdgeDistanceMinRange, _cameraEdgeInstantiationData[i].EdgeDistanceMaxRange);
                initialList.Add(go);
            }
            _totalAmountToSpawn += _cameraEdgeInstantiationData[i].AmountToCreate;
        }
        UnityUtils.Shuffle(initialList);
        initialList.ForEach(x => _objectPool.Push(x));
    }

    public IEnumerator SpawnGameObjectsPerRoutine()
    {
        int currentAmountRemaining = _totalAmountToSpawn;
        while (currentAmountRemaining > 0)
        {
            SpawnGameObjects(_amountToSpawnPerRoutine);
            currentAmountRemaining -= _amountToSpawnPerRoutine;
            yield return Helpers.GetWaitForSeconds(_spawnTimeInterval);
        }
        
    }

    private void SpawnGameObjects(int amountToSpawn)
    {
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
}
