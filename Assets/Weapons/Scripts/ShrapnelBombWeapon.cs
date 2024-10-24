using UnityEngine;

public class ShrapnelBombWeapon : Weapon
{
    [SerializeField] private ProjectileData _projectileData;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        Projectile proj = CreateProjectile(spawnPos, facingDirection, _projectileData);
    }
}