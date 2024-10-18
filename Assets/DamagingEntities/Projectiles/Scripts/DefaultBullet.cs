using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : Projectile
{
    public override void Move()
    {
        transform.position += transform.up * ProjectileSpeed * Time.fixedDeltaTime;
    }

    public override void OnHitCollision(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
