using UnityEngine;

public class ShrapnelBombWeapon : Weapon
{
    [SerializeField] private Projectile _projectile;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        Projectile proj = InstantiateProjectile(_projectile, spawnPos, facingDirection);
    }
}