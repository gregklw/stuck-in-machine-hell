using UnityEngine;

public class DoubleShotWeapon : Weapon
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Projectile _projectile;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        Vector3 perpDir = Vector2.Perpendicular(facingDirection);
        Projectile proj1 = InstantiateProjectile(_projectile, spawnPos + facingDirection * _offset.y - perpDir * _offset.x, facingDirection);
        Projectile proj2 = InstantiateProjectile(_projectile, spawnPos + facingDirection * _offset.y + perpDir * _offset.x, facingDirection);
    }
}