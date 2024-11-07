using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstPlayerParticleWeapon : PlayerParticleWeapon
{
    public override void SetAttackSpeed(float attackSpeed)
    {
        ParticleSystem.Burst burstModule = ThisParticleSystem.emission.GetBurst(0);
        burstModule.repeatInterval = Mathf.Pow(attackSpeed, AttackSpeedMultiplier);
    }

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
}
