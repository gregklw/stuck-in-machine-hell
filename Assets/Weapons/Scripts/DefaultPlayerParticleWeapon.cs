using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DefaultPlayerParticleWeapon : PlayerParticleWeapon
{
    protected override void OnSpecificParticleTrigger(List<ParticleSystem.Particle> particles, Collider2D collider)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            ParticleSystem.Particle p = particles[i];
            if (collider.bounds.Contains((Vector2)p.position))
            {
                collider.GetComponent<IHealthyObject>().ReceiveDamage(Damage);
                p.remainingLifetime = 0;
                particles[i] = p;
                ExplosionOnDestroy explosion = ExplosionObjectPool.Instance.GetNewExplosion(p.position, SkinData);
                explosion.InitSize(p.startSize3D);
                break;
            }
        }
    }

    public override void SetAttackSpeed(float attackSpeed)
    {
        ParticleSystem.EmissionModule emissionModule = ThisParticleSystem.emission;
        emissionModule.rateOverTime = Mathf.Pow(attackSpeed, AttackSpeedMultiplier);
        //Debug.Log(emissionModule.rateOverTime.constant);
    }
}
