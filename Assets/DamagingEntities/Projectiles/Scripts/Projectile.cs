using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private ExplosionOnDestroy _explosionOnDestroyPrefab;

    public void Init(Vector3 startDir, float damage)
    {
        _startDirection = startDir;
        transform.up = _startDirection;
        Damage = damage;
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
            DestroyProjectile();
        }
    }

    public abstract void Move();
    public abstract void OnHitCollision(Collider2D collision);

    public void OnTriggerEnter2D(Collider2D collision)
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

    private void DestroyProjectile()
    {
        ExplosionOnDestroy explosionOnDestroy = Instantiate(_explosionOnDestroyPrefab, transform.position, Quaternion.identity);
        explosionOnDestroy.InitSize(GetComponent<SpriteRenderer>());
        Destroy(gameObject);
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
