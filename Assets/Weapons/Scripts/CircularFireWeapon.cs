using UnityEngine;

public class CircularFireWeapon : Weapon
{
    [SerializeField] private ProjectileData _projectileData;
    [SerializeField][Range(0, 20)] private int _numberOfShrapnel;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        IProjectileBehaviour projectileBehaviour = new DefaultBullet();
        float angle = 0;
        for (int i = 0; i < _numberOfShrapnel; i++)
        {
            angle += (360 / (float)_numberOfShrapnel) * i;
            Projectile shrapnel = CreateProjectile(transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle), _projectileData, projectileBehaviour);
        }
    }
}
