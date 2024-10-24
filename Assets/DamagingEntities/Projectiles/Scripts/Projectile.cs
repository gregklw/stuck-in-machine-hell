using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public abstract class Projectile : MonoBehaviour, IDamagingEntity
{
    [Range(0.0f, 5.0f)][SerializeField] private float _projectileSpeed;
    public float ProjectileSpeed => _projectileSpeed;

    [Range(0.0f, 20.0f)][SerializeField] private float _projectileLifeTime;
    private float _lifeTimer;

    private Vector3 _startDirection;
    public float Damage { get; set; }
    [SerializeField] private string[] _collisionTagsForHit;
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private ProjectileData _projectileData;

    public ProjectileData ProjectileData => _projectileData;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollder;

    public void Init(Vector3 startDir, float damage, ProjectileData projectileData)
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _boxCollder ??= GetComponent<BoxCollider2D>();

        _spriteRenderer.sprite = projectileData.ProjectileSprite;
        _boxCollder.size = _spriteRenderer.bounds.size;
        _startDirection = startDir;
        transform.up = _startDirection;
        Damage = damage;
        _projectileData = projectileData;
        _projectileSpeed = _projectileData.ProjectileSpeed;
        _projectileLifeTime = _projectileData.ProjectileLifeTime;
        _collisionTagsForHit = _projectileData.CollisionTagsForHit;
        
    }

    private void FixedUpdate()
    {
        UpdateLifeTimer();
        Move();
    }

    private void UpdateLifeTimer()
    {
        _lifeTimer += Time.fixedDeltaTime;
        if (_lifeTimer > _projectileLifeTime)
        {
            RemoveAndCacheProjectile();
        }
    }

    public abstract void Move();
    public abstract void OnHitCollision(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHealthyObject>(out var damageable) && CompareTags(collision))
        {
            InflictDamage(damageable);
            OnHitCollision(collision);
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
        explosionOnDestroy.InitSize(GetComponent<SpriteRenderer>());
        ProjectilePool.Instance.AddUnusedProjectile(gameObject);
        Destroy(this);
    }

    private bool CompareTags(Collider2D collision)
    {
        foreach (string tag in _collisionTagsForHit)
        {
            if (collision.CompareTag(tag)) return true;
        }
        return false;
    }
}
