using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    [SerializeField] private ProjectileData _projectileData;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        Projectile proj = CreateProjectile(spawnPos, facingDirection, _projectileData);
    }
}
