using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillObjective : MonoBehaviour, IObjective
{
    private bool _isComplete;
    public bool IsComplete => _isComplete;

    private BusEventBinding<EnemyDeathEventWrapper> _killEvent;

    [SerializeField] private int _totalEnemiesRemaining;

    private void Awake()
    {
        WaveSpawner[] spawners = GetComponentsInChildren<WaveSpawner>();
        foreach (WaveSpawner spawner in spawners)
        {
            spawner.InitSpawner();
            _totalEnemiesRemaining += spawner.TotalAmountToSpawn;
        }
        _killEvent = new BusEventBinding<EnemyDeathEventWrapper>(DecrementEnemiesKilled);
    }

    private void OnEnable()
    {
        EventBus<EnemyDeathEventWrapper>.Register(_killEvent);
    }

    private void OnDisable()
    {
        EventBus<EnemyDeathEventWrapper>.Deregister(_killEvent);
    }

    private void DecrementEnemiesKilled(EnemyDeathEventWrapper enemyDeathEvent)
    {
        if (--_totalEnemiesRemaining <= 0)
        {
            _isComplete = true;
            EventBus<ObjectiveCompleteEventWrapper>.Raise(new ObjectiveCompleteEventWrapper());
            //EventBus<EnemyDeathEventWrapper>.Deregister(_killEvent);
        }
        //Debug.Log($"Enemies remaining: {_totalEnemiesRemaining} | Kill objective complete: {_isComplete}");
    }
}
