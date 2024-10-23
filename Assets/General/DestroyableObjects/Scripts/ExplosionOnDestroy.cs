using System;
using UnityEngine;

public class ExplosionOnDestroy : MonoBehaviour
{
    private SpriteRenderer _explosionRenderer;
    private Animator _explosionAnimator;
    private ExplosionObjectPool _explosionObjectPool;

    private void Awake()
    {
        _explosionRenderer = GetComponent<SpriteRenderer>();
        _explosionAnimator = GetComponent<Animator>();
    }

    public void SetExplosionVisuals(ExplosionVisuals explosionVisuals)
    {
        _explosionRenderer.sprite = explosionVisuals.ExplosionSprite;
        _explosionAnimator.runtimeAnimatorController = explosionVisuals.ExplosionAnimationController;
    }

    public void InitSize(SpriteRenderer destroyedObjectRenderer)
    {
        Vector3 explosionRendererSize = GetComponent<SpriteRenderer>().bounds.size;
        Vector3 destroyedRendererSize = destroyedObjectRenderer.bounds.size;
        float xAspectRatio = destroyedRendererSize.x / explosionRendererSize.x;
        float yAspectRatio = destroyedRendererSize.y / explosionRendererSize.y;

        float preferredDimension = destroyedRendererSize.x > destroyedRendererSize.y ? xAspectRatio : yAspectRatio;

        transform.localScale = new Vector3(preferredDimension, preferredDimension, preferredDimension);
    }

    public void DestroyObject()
    {
        _explosionRenderer.sprite = null;
        _explosionAnimator.runtimeAnimatorController = null;
        ExplosionObjectPool.Instance.AddUnusedExplosion(this);
    }
}
