using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerParticleWeapon : ParticleWeapon
{
    [SerializeField] private float _attackSpeedMultiplier;
    protected float AttackSpeedMultiplier => _attackSpeedMultiplier;
    public abstract void SetAttackSpeed(float attackSpeedValue);

    public bool IsUnlocked { get; set; }

    private BusEventBinding<EnemySpawnEventWrapper> _addEnemyColliderEventBinding;
    private BusEventBinding<EnemyDeathEventWrapper> _removeEnemyColliderEventBinding;

    protected override void Awake()
    {
        base.Awake();
        _addEnemyColliderEventBinding = new BusEventBinding<EnemySpawnEventWrapper>(AddCollider);
        _removeEnemyColliderEventBinding = new BusEventBinding<EnemyDeathEventWrapper>(RemoveCollider);
    }

    private void OnEnable()
    {
        EventBus<EnemySpawnEventWrapper>.Register(_addEnemyColliderEventBinding);
        EventBus<EnemyDeathEventWrapper>.Register(_removeEnemyColliderEventBinding);
    }

    private void OnDisable()
    {
        EventBus<EnemySpawnEventWrapper>.Deregister(_addEnemyColliderEventBinding);
        EventBus<EnemyDeathEventWrapper>.Deregister(_removeEnemyColliderEventBinding);
    }

    private void AddCollider(EnemySpawnEventWrapper enemySpawnEventWrapper)
    {
        AddCollider(enemySpawnEventWrapper.SpawnedEnemyCollider);
    }

    private void RemoveCollider(EnemyDeathEventWrapper enemyDeathEventWrapper)
    {
        RemoveCollider(enemyDeathEventWrapper.EnemyCollider);
    }
}
