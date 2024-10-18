using System;
using System.Collections;
using UnityEngine;

public abstract class DestroyableObject : MonoBehaviour, IHealthyObject
{
    public bool TakesNoDamage { get; set; }
    public abstract void AddHealth(float hpToAdd);
    public abstract void ReceiveDamage(float damage);

    [SerializeField] private float _currentHealth, _maxHealth;
    [SerializeField] private ExplosionOnDestroy _destroyedAnimationPrefab;
    public Action OnDestroyEvent;
    public float CurrentHealth
    {
        get => _currentHealth;
        protected set
        {
            if (value <= 0)
            {
                _currentHealth = 0;
                DestroyObject();
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

    private void DestroyObject()
    {
        InstantiateDestroyedAnimation();
        OnDestroyEvent?.Invoke();
        Destroy(gameObject);
    }

    private void InstantiateDestroyedAnimation()
    {
        if (_destroyedAnimationPrefab != null)
        {
            Instantiate(_destroyedAnimationPrefab, transform.position, Quaternion.identity).InitSize(
                GetComponentInChildren<SpriteRenderer>()
            );
        }
    }
}
