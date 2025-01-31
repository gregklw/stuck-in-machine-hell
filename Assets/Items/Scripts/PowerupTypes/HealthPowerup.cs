using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{
    public int HealthValue;
    public override void ActivatePowerup(Player player)
    {
        player.AddHealth(HealthValue);
    }
}
