using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that creates other shrapnel bullets when hit on impact
/// </summary>
public class ShrapnelBomb : IProjectileBehaviour
{
    private int _shrapnelDamage;
    private int _numberOfShrapnel;
    private ProjectileData _shrapnelData;
    private Transform _baseProjectileT;

    public ShrapnelBomb(Transform baseProjectileT, int shrapnelDamage, int numberOfShrapnel, ProjectileData shrapnelData)
    {
        _baseProjectileT = baseProjectileT;
        _shrapnelDamage = shrapnelDamage;
        _numberOfShrapnel = numberOfShrapnel;
        _shrapnelData = shrapnelData;
    }

    public void Move(Transform projectileTransform, float projectileSpeed)
    {
        projectileTransform.position += projectileTransform.up * projectileSpeed * Time.fixedDeltaTime;
    }

    public void OnHitCollision()
    {
        float angle = 0;
        for (int i = 0; i < _numberOfShrapnel; i++)
        {
            angle += (360 / (float) _numberOfShrapnel) * i;

            Projectile shrapnelSpawn = ProjectilePool.Instance.GetNewProjectile();
            shrapnelSpawn.SetProjectileBehaviour(new DefaultBullet());
            Transform shrapnelSpawnT = shrapnelSpawn.transform;
            shrapnelSpawnT.position = _baseProjectileT.position;
            shrapnelSpawn.Init(CommonCalculations.RotateTowardsUp(Vector3.up, angle), _shrapnelDamage, _shrapnelData);
        }
        //ProjectilePool.Instance.AddUnusedProjectile(gameObject);
        //Destroy(this);
    }
}
