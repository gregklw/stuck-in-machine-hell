using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//should progressively spawn things one by one
public class SpawningSequencer : MonoBehaviour
{
    [SerializeField] private List<WaveSpawner> _spawners = new List<WaveSpawner>();

    private IEnumerator Start()
    {
        _spawners = GetComponentsInChildren<WaveSpawner>().ToList();

        foreach (var spawner in _spawners)
        {
            Debug.Log("RUN");
            yield return spawner.SpawnGameObjectsPerRoutine();
        }
    }
}
