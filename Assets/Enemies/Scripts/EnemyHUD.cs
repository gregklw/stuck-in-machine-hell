using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private AttributeBar _healthBar;
    [SerializeField] private Enemy _enemy;
    private const float BarWidthMultiplier = 1.5f;

    private void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        _healthBar.SetBarDisplayWidth((boxCollider.bounds.size.x * BarWidthMultiplier));
        _healthBar.BarPosition = transform.position;
        _healthBar.BarPosition -= (Vector2)(transform.up * boxCollider.bounds.size.y);
        _healthBar.RealignBar();
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
