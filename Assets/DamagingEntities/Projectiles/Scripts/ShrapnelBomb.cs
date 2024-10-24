using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that creates other shrapnel bullets when hit on impact
/// </summary>
public class ShrapnelBomb : Projectile
{
    [SerializeField][Range(0,100)] private int _shrapnelDamage;
    [SerializeField][Range(0, 32)] private int _numberOfShrapnel;
    [SerializeField] private Projectile _shrapnelPrefab;
    public override void Move()
    {
        transform.position += transform.up * ProjectileSpeed * Time.fixedDeltaTime;
    }

    public override void OnHitCollision(Collider2D collision)
    {
        float angle = 0;
        for (int i = 0; i < _numberOfShrapnel; i++)
        {
            angle += (360 / (float) _numberOfShrapnel) * i;

            GameObject projBase = ProjectilePool.Instance.GetNewProjectile();
            projBase.transform.position = transform.position;
            Projectile proj = projBase.AddComponent<DefaultBullet>();
            proj.Init(CommonCalculations.RotateTowardsUp(Vector3.up, angle), Damage, ProjectileData);
            proj.Damage = _shrapnelDamage;
        }
        //ProjectilePool.Instance.AddUnusedProjectile(gameObject);
        //Destroy(this);
        RemoveAndCacheProjectile();
    }
}
