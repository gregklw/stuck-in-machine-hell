using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelBombPlayerParticleWeapon : PlayerParticleWeapon
{
    [SerializeField] private int _numberOfShrapnel;
    [SerializeField] private PlayerParticleWeapon _shrapnelWeapon;
    public override void SetAttackSpeed(float attackSpeed)
    {
        ParticleSystem.EmissionModule emissionModule = ThisParticleSystem.emission;
        emissionModule.rateOverTime = Mathf.Pow(attackSpeed, AttackSpeedMultiplier);
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
                _shrapnelWeapon.transform.position = p.position;
                _shrapnelWeapon.FireBurstParticlesOnce(_numberOfShrapnel);
                //ExplosionObject explosion = ExplosionObjectPool.Instance.GetNewExplosion(p.position, SkinData.ExplosionSprite, SkinData.ExplosionAnimationController);
                //explosion.InitSize(p.startSize3D);
                TriggerExplosionEffect(p.position, p.startSize);
                particles[i] = p;
                break;
            }
        }
    }
}
