using UnityEngine;

public class ShrapnelBombWeapon : Weapon
{
    [SerializeField] private ProjectileData _projectileData, _shrapnelProjectileData;
    [SerializeField][Range(0, 100)] private int _shrapnelDamage;
    [SerializeField][Range(0, 32)] private int _numberOfShrapnel;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        IProjectileBehaviour projectileBehaviour = new ShrapnelBomb(transform, _shrapnelDamage, _numberOfShrapnel, _shrapnelProjectileData);
        Projectile projBase = CreateProjectile(spawnPos, facingDirection, _projectileData, projectileBehaviour);
    }
}