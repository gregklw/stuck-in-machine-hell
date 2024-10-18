using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : AttackingCharacter
{
    private Player _targetPlayer;
    public int Score;
    public Action<EnemyHealthEventWrapper> HealthEvent;

    protected Player TargetPlayer => _targetPlayer;

    private void Awake()
    {
        //HealthEventBinding = new LocalEventBinding<EnemyEvent>();
        _targetPlayer = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        OnDestroyEvent += TriggerScoring;
    }

    private void OnDisable()
    {
        OnDestroyEvent -= TriggerScoring;
    }


    private void TriggerScoring()
    {
        EventBus<EnemyDeathEventWrapper>.Raise(
            new EnemyDeathEventWrapper
            {
                Score = this.Score
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

    public abstract void ShootProjectile();
}
