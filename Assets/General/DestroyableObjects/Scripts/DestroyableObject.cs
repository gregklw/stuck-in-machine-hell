using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class DestroyableObject : MonoBehaviour, IHealthyObject
{
    public bool TakesNoDamage { get; set; }
    public abstract void AddHealth(float hpToAdd);
    public abstract void ReceiveDamage(float damage);

    [SerializeField] private float _currentHealth, _maxHealth;
    [SerializeField] private ProjectileSkinData _projectileSkinData;
    [SerializeField] private ExplosionEmitter _deathExplosionEmitter;
    private SpriteRenderer _characterRenderer;
    protected SpriteRenderer CharacterRenderer => _characterRenderer;
    public Action OnDestroyEvent;
    public float CurrentHealth
    {
        get => _currentHealth;
        protected set
        {
            if (value <= 0)
            {
                _currentHealth = 0;
                StartCoroutine(DestroyObject());
            }
            else if (value > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            else
            {
                _currentHealth = value;
            }
        }

    }
    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public ArmorType ArmorType => _armorType;

    [SerializeField] private ArmorType _armorType;

    protected virtual void Awake()
    {
        _characterRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator DestroyObject()
    {
        TriggerExplosion();
        OnDestroyEvent?.Invoke();
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void TriggerExplosion()
    {
        //ExplosionObject explosion = ExplosionObjectPool.Instance.GetNewExplosion(transform.position, _deathAnimatorController);
        //ExplosionEmitter explosion = ExplosionEmitterPool.Instance.GetCachedExplosionObject(_projectileSkinData.DeathExplosionMaterial);
        _deathExplosionEmitter.InitAnimation(GetComponent<Collider2D>().bounds.size.x * 2, _projectileSkinData.DeathExplosionMaterial, _projectileSkinData.SpriteSheetRows, _projectileSkinData.SpriteSheetColumns);
        _deathExplosionEmitter.ExplodeOnLocation(transform.position);

        IParticleSystemHelper[] particlesToDetachAndDestroy = GetComponentsInChildren<IParticleSystemHelper>();

        Array.ForEach<IParticleSystemHelper>(particlesToDetachAndDestroy, x => StartCoroutine(x.DetachAndDestroyWhenParticlesDead()));
    }
}
