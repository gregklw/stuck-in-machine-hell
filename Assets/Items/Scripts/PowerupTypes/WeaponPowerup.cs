using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerup : Powerup
{
    [SerializeField] private Weapon _weaponPrefab;
    public Weapon WeaponPrefab => _weaponPrefab;
    public override void ActivatePowerup(Player player)
    {
        //consider destroying weapon argument or prevent instantiation when weapon already exists
        EventBus<WeaponPickupEventWrapper>.Raise(
            new WeaponPickupEventWrapper
            {
                WeaponPrefab = _weaponPrefab
            }
        );
    }
}
