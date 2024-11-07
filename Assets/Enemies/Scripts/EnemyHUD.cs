using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    //private const float BarWidthMultiplier = 1.5f;
    private IAttributeBar _healthBar;

    private void Start()
    {
        _healthBar = GetComponentInChildren<IAttributeBar>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        _healthBar.SetBarDisplayWidth((boxCollider.bounds.size.x));
        _healthBar.BarPosition = transform.position;
        _healthBar.BarPosition -= (Vector2)(transform.up * boxCollider.bounds.size.y) /2;
        _healthBar.CenterBarPosX();
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
