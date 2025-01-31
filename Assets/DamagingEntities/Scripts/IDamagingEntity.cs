using UnityEngine;

//layer of abstraction for loose coupling when something damaging interacts with something that can be damaged
public interface IDamagingEntity
{
    float Damage { get; set; }
    void InflictDamage(IHealthyObject damageable);
}
