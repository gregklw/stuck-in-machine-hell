using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleWeapon : ParticleWeapon
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
                ExplosionObject explosion = ExplosionObjectPool.Instance.GetNewExplosion(p.position, SkinData);
                explosion.InitSize(p.startSize3D);
                particles[i] = p;
                break;
            }
        }
    }
}
