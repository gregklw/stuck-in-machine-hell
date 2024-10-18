using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Weapon : Weapon
{
    [SerializeField] private Projectile[] projectilePrefabs;
    private int _projectilePrefabSelectionCount;
    private int _angleOffset;
    private const int RotateOffsetAmount = 20;
    private const int FullRotationValue = 360;

    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        if (_angleOffset % FullRotationValue == 0) _angleOffset = 0;
        if (_projectilePrefabSelectionCount % projectilePrefabs.Length == 0) _projectilePrefabSelectionCount = 0;

        float angle = _angleOffset;

        if (_projectilePrefabSelectionCount % projectilePrefabs.Length == 0) _projectilePrefabSelectionCount = 0;
        InstantiateProjectile(projectilePrefabs[_projectilePrefabSelectionCount++], transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle));

        if (_projectilePrefabSelectionCount % projectilePrefabs.Length == 0) _projectilePrefabSelectionCount = 0;
        InstantiateProjectile(projectilePrefabs[_projectilePrefabSelectionCount++], transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90));

        if (_projectilePrefabSelectionCount % projectilePrefabs.Length == 0) _projectilePrefabSelectionCount = 0;
        InstantiateProjectile(projectilePrefabs[_projectilePrefabSelectionCount++], transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90));

        if (_projectilePrefabSelectionCount % projectilePrefabs.Length == 0) _projectilePrefabSelectionCount = 0;
        InstantiateProjectile(projectilePrefabs[_projectilePrefabSelectionCount++], transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle += 90));

        _angleOffset += RotateOffsetAmount;
    }

}
