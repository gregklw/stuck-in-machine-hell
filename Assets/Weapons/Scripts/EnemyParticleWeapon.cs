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
                //ExplosionEmitter explosion = ExplosionEmitterPool.Instance.GetCachedExplosionObject(SkinData.DeathExplosionMaterial);
                ThisExplosionEmitter.InitAnimation(p.startSize3D.x, SkinData.DeathExplosionMaterial, SkinData.SpriteSheetRows, SkinData.SpriteSheetColumns);
                ThisExplosionEmitter.ExplodeOnLocation(p.position);
                particles[i] = p;
                break;
            }
        }
    }
}
