using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For changing the stats of player weapons whenever player picks up powerup that affects weapons.
/// </summary>
public struct WeaponStatModifierEventWrapper : IEventWrapper
{
    public float AttackSpeedValue;
}
