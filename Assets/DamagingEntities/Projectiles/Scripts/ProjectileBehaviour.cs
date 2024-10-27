using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using strategy pattern to separate bullet logic.
/// </summary>
public interface IProjectileBehaviour 
{
    void Move(Transform projectileTransform, float projectileSpeed);
    void OnHitCollision();
}
