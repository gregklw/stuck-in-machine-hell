using System;
using System.Collections;
using UnityEngine;

public class ExplosionEmitter : MonoBehaviour, IParticleSystemHelper
{
    private ParticleSystem _explosionParticleSystem;
    private ParticleSystemRenderer _explosionParticleRenderer;
    private Animator _explosionAnimator;

    private void Awake()
    {
        _explosionParticleSystem = GetComponent<ParticleSystem>();
        _explosionParticleRenderer = GetComponent<ParticleSystemRenderer>();
        _explosionAnimator = GetComponent<Animator>();
    }

    public void ExplodeOnLocation(Vector2 location)
    {
        transform.position = location;
        _explosionParticleSystem.Emit(1);
    }

    public void InitAnimation(float targetSize, Material deathAnimationMaterial, int rows, int columns)
    {
        //float preferredDimension = targetSize.x > targetSize.y ? targetSize.y : targetSize.x;

        ParticleSystem.MainModule main = _explosionParticleSystem.main;
        main.startSize = targetSize;

        ParticleSystem.TextureSheetAnimationModule tsaModule = _explosionParticleSystem.textureSheetAnimation;

        tsaModule.numTilesX = rows;
        tsaModule.numTilesY = columns;

        _explosionParticleRenderer.material = deathAnimationMaterial;
    }

    public IEnumerator DetachAndDestroyWhenParticlesDead()
    {
        _explosionParticleSystem.Stop();
        transform.SetParent(null);
        while (_explosionParticleSystem.IsAlive())
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    //public void ResetExplosionAnimator()
    //{
    //    _explosionRenderer.sprite = null;
    //    _explosionAnimator.runtimeAnimatorController = null;
    //}
}
