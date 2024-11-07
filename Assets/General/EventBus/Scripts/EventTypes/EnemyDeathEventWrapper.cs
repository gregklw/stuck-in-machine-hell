using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyDeathEventWrapper : IEventWrapper
{
    public int Score;
    public Collider2D EnemyCollider;
}
