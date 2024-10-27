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

    public void SetExplosionVisuals(ProjectileData explosionVisuals)
    {
        _explosionRenderer.sprite = explosionVisuals.ExplosionSprite;
        _explosionAnimator.runtimeAnimatorController = explosionVisuals.ExplosionAnimationController;
    }

    public void InitSize(Vector3 targetSize)
    {
        Vector3 explosionRendererSize = _explosionRenderer.size;

        float xAspectRatio = targetSize.x / explosionRendererSize.x;
        float yAspectRatio = targetSize.y / explosionRendererSize.y;

        float preferredDimension = targetSize.x > targetSize.y ? xAspectRatio : yAspectRatio;
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(preferredDimension, preferredDimension, preferredDimension);
    }

    //public void ResetExplosionAnimator()
    //{
    //    _explosionRenderer.sprite = null;
    //    _explosionAnimator.runtimeAnimatorController = null;
    //}
}
