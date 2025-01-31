using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingPlayerParticleWeapon : PlayerParticleWeapon
{
    protected override void OnSpecificParticleTrigger(List<ParticleSystem.Particle> particles, Collider2D collider)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            ParticleSystem.Particle p = particles[i];
            if (collider.bounds.Contains((Vector2)p.position))
            {
                collider.GetComponent<IHealthyObject>().ReceiveDamage(Damage);
            }
        }
    }

    public override void SetAttackSpeed(float attackSpeed)
    {
        ParticleSystem.EmissionModule emissionModule = ThisParticleSystem.emission;
        emissionModule.rateOverTime = Mathf.Pow(attackSpeed, AttackSpeedMultiplier);
    }
}
