using UnityEngine;

public class DoubleShotWeapon : Weapon
{

    [SerializeField] private Vector2 _offset;
    [SerializeField] private ProjectileData projectileData1;
    [SerializeField] private ProjectileData projectileData2;

    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        Vector3 perpDir = Vector2.Perpendicular(facingDirection);
        Projectile proj1 = CreateProjectile(spawnPos + facingDirection * _offset.y - perpDir * _offset.x, facingDirection, projectileData1);
        Projectile proj2 = CreateProjectile(spawnPos + facingDirection * _offset.y + perpDir * _offset.x, facingDirection, projectileData2);
    }
}