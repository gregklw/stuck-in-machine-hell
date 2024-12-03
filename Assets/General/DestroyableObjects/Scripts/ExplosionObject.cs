using System;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    private SpriteRenderer _explosionRenderer;
    private Animator _explosionAnimator;
    private ExplosionObjectPool _explosionObjectPool;
    private string _defaultAnimationName; //save default animation name for reuse

    private void Awake()
    {
        _explosionRenderer = GetComponent<SpriteRenderer>();
        _explosionAnimator = GetComponent<Animator>();
        _defaultAnimationName = _explosionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name; //save default animation name for reuse
    }

    public void SetExplosionVisuals(RuntimeAnimatorController animationController)
    {
        //_explosionRenderer.sprite = startingSprite;
        _explosionAnimator.runtimeAnimatorController = animationController;
        _explosionAnimator.Play(_defaultAnimationName);
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
