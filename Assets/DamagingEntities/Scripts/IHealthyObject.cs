using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthyObject
{
    bool TakesNoDamage { get; set; }
    void AddHealth(float hpToAdd);
    void ReceiveDamage(float damage);
    ArmorType ArmorType { get;}
}
