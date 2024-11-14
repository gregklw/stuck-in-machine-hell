using System;
using UnityEngine;

public abstract class Enemy : AttackingCharacter
{
    private Player _targetPlayer;
    public int Score;
    public Action<EnemyHealthEventWrapper> HealthEvent;

    protected Player TargetPlayer => _targetPlayer;

    protected override void Awake()
    {
        base.Awake();
        //HealthEventBinding = new LocalEventBinding<EnemyEvent>();
        _targetPlayer = FindObjectOfType<Player>();
    }

    private void Start()
    {
        EventBus<EnemySpawnEventWrapper>.Raise(
            new EnemySpawnEventWrapper()
            {
                SpawnedEnemyCollider = GetComponent<Collider2D>(),
                EnemyParticleWeapons = GetComponentsInChildren<EnemyParticleWeapon>()
            }
        );
    }

    private void OnEnable()
    {
        //FindObjectOfType<PlayerParticleWeapon>().AddCollider(GetComponent<Collider2D>());
        OnDestroyEvent += TriggerScoring;
    }

    private void OnDisable()
    {
        //FindObjectOfType<PlayerParticleWeapon>().RemoveCollider(GetComponent<Collider2D>());
        OnDestroyEvent -= TriggerScoring;
    }


    private void TriggerScoring()
    {
        EventBus<EnemyDeathEventWrapper>.Raise(
            new EnemyDeathEventWrapper
            {
                Score = this.Score,
                EnemyCollider = GetComponent<Collider2D>()
            }
        );
      
    }

    public override void ReceiveDamage(float damage)
    {
        this.CurrentHealth -= damage;
        RaiseHealthEvent();
    }

    public override void AddHealth(float hpToAdd)
    {
        this.CurrentHealth += hpToAdd;
        RaiseHealthEvent();
    }

    private void RaiseHealthEvent()
    {
        HealthEvent?.Invoke(new EnemyHealthEventWrapper
        {
            CurrentHealth = this.CurrentHealth,
            MaxHealth = this.MaxHealth
        });
    }

    public Vector2 GetDirectionVectorTowardsPlayer()
    {
        return TargetPlayer.transform.position - transform.position;
    }
}
