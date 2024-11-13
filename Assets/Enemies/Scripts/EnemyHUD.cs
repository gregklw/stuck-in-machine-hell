using Unity.VisualScripting;
using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SpriteRenderer _enemySprite;
    //private const float BarWidthMultiplier = 1.5f;
    private IAttributeBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<IAttributeBar>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Vector2 unitSizeAmount = _enemySprite.sprite.rect.size / _enemySprite.sprite.pixelsPerUnit * _enemySprite.transform.localScale;
        Vector2 barPosition = transform.position - (transform.up * unitSizeAmount.y) / 2;
        _healthBar.SetupBar(unitSizeAmount.x, barPosition);
    }

    private void OnEnable()
    {
        _enemy.HealthEvent += HandleHealthEvent;
    }

    private void OnDisable()
    {
        _enemy.HealthEvent -= HandleHealthEvent;
    }

    private void HandleHealthEvent(EnemyHealthEventWrapper enemyHealthEvent)
    {
        _healthBar.SetBarAmount(enemyHealthEvent.CurrentHealth / enemyHealthEvent.MaxHealth);
    }
}
