using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : IProjectileBehaviour
{
    public void Move(Transform projectileTransform, float projectileSpeed)
    {
        projectileTransform.position += projectileTransform.up * projectileSpeed * Time.fixedDeltaTime;
    }

    public void OnHitCollision()
    {
    }
}
