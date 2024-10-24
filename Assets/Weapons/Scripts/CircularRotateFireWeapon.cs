using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularRotateFireWeapon : Weapon
{
    [SerializeField] private ProjectileData _projectileData;

    private int _angleOffset;
    private const int RotateOffsetAmount = 20;
    private const int FullRotationValue = 360;

    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        if (_angleOffset % FullRotationValue == 0) _angleOffset = 0;
        float angle = _angleOffset;
        Projectile shrapnel = CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle), _projectileData);
        _angleOffset += RotateOffsetAmount;
    }
}
