using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Weapon : Weapon
{
    [SerializeField] private ProjectileData[] bossProjectileData;
    private int _projectilePrefabSelectionCount;
    private int _angleOffset;
    private const int RotateOffsetAmount = 20;
    private const int FullRotationValue = 360;

    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        if (_angleOffset % FullRotationValue == 0) _angleOffset = 0;
        if (_projectilePrefabSelectionCount % bossProjectileData.Length == 0) _projectilePrefabSelectionCount = 0;

        float angle = _angleOffset;

        IProjectileBehaviour projectileBehaviour = new DefaultBullet();

        if (_projectilePrefabSelectionCount % bossProjectileData.Length == 0) _projectilePrefabSelectionCount = 0;
        CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle), bossProjectileData[_projectilePrefabSelectionCount++], projectileBehaviour);

        if (_projectilePrefabSelectionCount % bossProjectileData.Length == 0) _projectilePrefabSelectionCount = 0;
        CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90), bossProjectileData[_projectilePrefabSelectionCount++], projectileBehaviour);

        if (_projectilePrefabSelectionCount % bossProjectileData.Length == 0) _projectilePrefabSelectionCount = 0;
        CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90), bossProjectileData[_projectilePrefabSelectionCount++], projectileBehaviour);

        if (_projectilePrefabSelectionCount % bossProjectileData.Length == 0) _projectilePrefabSelectionCount = 0;
        CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90), bossProjectileData[_projectilePrefabSelectionCount++], projectileBehaviour);

        _angleOffset += RotateOffsetAmount;
    }
}
