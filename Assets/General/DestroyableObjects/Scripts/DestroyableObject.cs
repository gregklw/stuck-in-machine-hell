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
    [SerializeField] private RuntimeAnimatorController _deathAnimatorController;
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
        InstantiateDestroyedAnimation();
        OnDestroyEvent?.Invoke();
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void InstantiateDestroyedAnimation()
    {
        ExplosionObject explosion = ExplosionObjectPool.Instance.GetNewExplosion(transform.position, _deathAnimatorController);
        explosion.InitSize(GetComponent<Collider2D>().bounds.size);
    }
}
