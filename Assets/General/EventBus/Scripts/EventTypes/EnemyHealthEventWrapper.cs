using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using structs over classes because structs are allocated on the stack not the heap
public struct EnemyHealthEventWrapper : IEventWrapper
{
    //enemy can lose health, gain health, do damage, give score to player
    public float CurrentHealth;
    public float MaxHealth;
}
