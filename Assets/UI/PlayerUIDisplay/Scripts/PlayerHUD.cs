using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private IAttributeBar _healthBar;

    private BusEventBinding<PlayerHealthEventWrapper> _healthEventBinding;
    private BusEventBinding<EnemyDeathEventWrapper> _scoringEventBinding;

    private int _score;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<IAttributeBar>();
        _healthEventBinding = new BusEventBinding<PlayerHealthEventWrapper>(HandleHealthEvent);
        _scoringEventBinding = new BusEventBinding<EnemyDeathEventWrapper>(HandleScoringEvent);
    }

    private void OnEnable()
    {
        EventBus<PlayerHealthEventWrapper>.Register(_healthEventBinding);
        EventBus<EnemyDeathEventWrapper>.Register(_scoringEventBinding);
    }

    private void OnDisable()
    {
        EventBus<PlayerHealthEventWrapper>.Deregister(_healthEventBinding);
        EventBus<EnemyDeathEventWrapper>.Deregister(_scoringEventBinding);
    }

    private void HandleHealthEvent(PlayerHealthEventWrapper playerEvent)
    {
        _healthBar.SetBarAmount(playerEvent.CurrentHealth / playerEvent.MaxHealth);
    }

    private void HandleScoringEvent(EnemyDeathEventWrapper scoringEvent)
    {
        _score += scoringEvent.Score;
        _scoreText.text = $"Score: {_score}";
    }
}
