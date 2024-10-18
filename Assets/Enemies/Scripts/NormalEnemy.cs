using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField][Range(0, 5)] private float _baseMoveSpeed;
    public float BaseMoveSpeed
    {
        get => _baseMoveSpeed;
        set => _baseMoveSpeed = value;
    }
    private float _counter;
    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        ShootProjectile();
    }

    public override void ShootProjectile()
    {
        _counter += Time.fixedDeltaTime;
        if (_counter > CommonCalculations.CalculateAttackSpeed(BaseAttackSpeed))
        {
            CurrentWeapon.Fire(transform.position, transform.up);
            _counter = 0;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!TargetPlayer) return;
        transform.up = GetDirectionVectorTowardsPlayer();
        transform.position += transform.up * BaseMoveSpeed * Time.fixedDeltaTime;
    }
}
