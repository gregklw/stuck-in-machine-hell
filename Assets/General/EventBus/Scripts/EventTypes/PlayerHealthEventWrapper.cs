using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerHealthEventWrapper : IEventWrapper
{
    public float CurrentHealth;
    public float MaxHealth;
}
