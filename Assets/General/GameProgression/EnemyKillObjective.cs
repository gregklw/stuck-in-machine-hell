using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillObjective : MonoBehaviour, IObjective
{
    private bool _isComplete;
    public bool IsComplete => _isComplete;

    public bool IsGameOver { get; set; }

    private BusEventBinding<EnemyDeathEventWrapper> _killEvent;
    private BusEventBinding<PlayerDeathEventWrapper> _playerDeathEvent;

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
        _playerDeathEvent = new BusEventBinding<PlayerDeathEventWrapper>(DisableObjective);
    }

    private void DisableObjective(PlayerDeathEventWrapper wrapper)
    {
        wrapper.SetGameOver(this);
    }

    private void OnEnable()
    {
        EventBus<EnemyDeathEventWrapper>.Register(_killEvent);
        EventBus<PlayerDeathEventWrapper>.Register(_playerDeathEvent);
    }

    private void OnDisable()
    {
        EventBus<EnemyDeathEventWrapper>.Deregister(_killEvent);
        EventBus<PlayerDeathEventWrapper>.Deregister(_playerDeathEvent);
    }

    private void DecrementEnemiesKilled(EnemyDeathEventWrapper enemyDeathEvent)
    {
        if (IsGameOver) return;

        if (--_totalEnemiesRemaining <= 0)
        {
            _isComplete = true;
            EventBus<ObjectiveCompleteEventWrapper>.Raise(new ObjectiveCompleteEventWrapper());
            //EventBus<EnemyDeathEventWrapper>.Deregister(_killEvent);
        }
        //Debug.Log($"Enemies remaining: {_totalEnemiesRemaining} | Kill objective complete: {_isComplete}");
    }
}
