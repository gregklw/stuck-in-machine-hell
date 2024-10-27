using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamagingEntity
{
    private float _projectileSpeed;
    public float ProjectileSpeed => _projectileSpeed;

    private float _projectileLifeTime;
    private float _lifeTimer;

    public float Damage { get; set; }
    private string[] _collisionTagsForHit;
    private ProjectileType _projectileType;
    [SerializeField] private ProjectileData _projectileData;

    public ProjectileData ProjectileData => _projectileData;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollder;

    private IProjectileBehaviour _projectileBehaviour; //ProjectileBehaviour uses strategy pattern or state pattern to separate logic

    public void Init(Vector3 startDir, float damage, ProjectileData projectileData)
    {
        _spriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
        _boxCollder ??= GetComponent<BoxCollider2D>();
        _projectileData = projectileData;
        _projectileType = _projectileData.ProjectileType;
        DamageTypesUtil.SetProjectileColliderSize(_boxCollder, _projectileType);
        _spriteRenderer.sprite = projectileData.ProjectileSprite;
        SetUnitSizeForSprite(_spriteRenderer);
        transform.up = startDir;
        Damage = damage;
        _projectileSpeed = _projectileData.ProjectileSpeed;
        _projectileLifeTime = _projectileData.ProjectileLifeTime;
        _collisionTagsForHit = _projectileData.CollisionTagsForHit;
    }

    public void ResetProjectile()
    {
        _lifeTimer = 0;
    }

    public void SetProjectileBehaviour(IProjectileBehaviour projectileBehaviour)
    {
        _projectileBehaviour = projectileBehaviour;
    }

    private void FixedUpdate()
    {
        UpdateLifeTimer();
        _projectileBehaviour.Move(transform, ProjectileSpeed);
    }

    private void UpdateLifeTimer()
    {
        _lifeTimer += Time.fixedDeltaTime;
        if (_lifeTimer > _projectileLifeTime)
        {
            RemoveAndCacheProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHealthyObject>(out var damageable) && CompareTags(collision))
        {
            InflictDamage(damageable);
            _projectileBehaviour.OnHitCollision();
            RemoveAndCacheProjectile();
        }
    }

    public void InflictDamage(IHealthyObject damageable)
    {
        damageable.ReceiveDamage(CommonCalculations.CalculateArmorAmplify(Damage, damageable.ArmorType, _projectileType));
    }

    public void RemoveAndCacheProjectile()
    {
        ExplosionOnDestroy explosionOnDestroy = ExplosionObjectPool.Instance.GetNewExplosion(_projectileData);
        explosionOnDestroy.transform.position = transform.position;
        explosionOnDestroy.InitSize(_boxCollder.bounds.size);
        ProjectilePool.Instance.CacheUnusedProjectile(this);
    }

    private bool CompareTags(Collider2D collision)
    {
        foreach (string tag in _collisionTagsForHit)
        {
            if (collision.CompareTag(tag)) return true;
        }
        return false;
    }

    private void SetUnitSizeForSprite(SpriteRenderer renderer)
    {
        Transform rendererT = renderer.transform;
        Sprite rendererSprite = renderer.sprite;
        rendererT.localScale = Vector3.one;
        rendererT.localScale = (rendererT.localScale / rendererSprite.rect.size) * rendererSprite.pixelsPerUnit * _boxCollder.size;
    }
}
