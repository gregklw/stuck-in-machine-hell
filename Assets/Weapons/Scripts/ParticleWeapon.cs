using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleWeapon : MonoBehaviour, IParticleSystemHelper
{
    [SerializeField] private ProjectileSkinData _projectileData;
    [SerializeField] private float _damage;
    [SerializeField] private LayerMask _hitTriggerLayer;
    [SerializeField] private ExplosionEmitter _explosionEmitter;

    private List<ParticleSystem.Particle> _insideParticles = new List<ParticleSystem.Particle>();
    private ParticleSystem _particleSystem;

    protected ParticleSystem ThisParticleSystem => _particleSystem;
    protected ExplosionEmitter ThisExplosionEmitter => _explosionEmitter;
    protected ProjectileSkinData SkinData => _projectileData;
    protected float Damage => _damage;
    protected virtual void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        //_registerColliderEventBinding = new BusEventBinding<T>(RegisterCollider);
        Sprite[] sprites = _projectileData.ProjectileSprites;
        //Array.ForEach(sprites, sprite => _particleSystem.textureSheetAnimation.SetSprite(0, _projectileData.ProjectileSprite));
        for (int i = 0; i < sprites.Length; i++)
        {
            _particleSystem.textureSheetAnimation.SetSprite(i, sprites[i]);
        }
    }

    private void OnParticleTrigger()
    {
        bool isLayerMatching;
        ParticleSystem.TriggerModule particleTriggerModule = _particleSystem.trigger;
        for (int i = 0; i < particleTriggerModule.colliderCount; i++)
        {
            if (particleTriggerModule.GetCollider(i) == null)
            {
                Debug.Log("Collider doesn't exist/is already destroyed");
                particleTriggerModule.RemoveCollider(i);
                continue;
            }
            //Debug.Log("COLLIDER TOUCHES");

            Collider2D collider = particleTriggerModule.GetCollider(i).GetComponent<Collider2D>();
            isLayerMatching = (_hitTriggerLayer.value & (1 << collider.gameObject.layer)) != 0;
            if (isLayerMatching)
            {
                _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, _insideParticles);
                OnSpecificParticleTrigger(_insideParticles, collider);
                _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, _insideParticles); //MUST SET PARTICLES AFTERWARDS FOR EFFECT
            }
        }
    }

    protected abstract void OnSpecificParticleTrigger(List <ParticleSystem.Particle> particles, Collider2D collider);

    public void AddCollider(Component component)
    {
        _particleSystem.trigger.AddCollider(component.GetComponent<Collider2D>());
    }

    public void RemoveCollider(Component component)
    {
        _particleSystem.trigger.RemoveCollider(component.GetComponent<Collider2D>());
    }

    public void StartGun()
    {
        _particleSystem.Play();
    }

    public void StopGun()
    {
        _particleSystem.Stop();
    }

    public void FireBurstParticlesOnce(int count)
    {
        _particleSystem.Emit(count);
    }

    public void TriggerExplosionEffect(Vector2 spawnPosition, float startSize)
    {
        _explosionEmitter.InitAnimation(startSize, _projectileData.DeathExplosionMaterial, _projectileData.SpriteSheetRows, _projectileData.SpriteSheetColumns);
        _explosionEmitter.ExplodeOnLocation(spawnPosition);
    }

    public IEnumerator DetachAndDestroyWhenParticlesDead()
    {
        transform.SetParent(null);
        _particleSystem.Stop();
        while (_particleSystem.IsAlive())
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
