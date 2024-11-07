using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemySpawnEventWrapper : IEventWrapper
{
    public Collider2D SpawnedEnemyCollider;
    public EnemyParticleWeapon[] EnemyParticleWeapons;
}
