using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : AttackingCharacter
{
    private Camera _mainCamera;
    private Camera MainCamera
    {
        get => _mainCamera ??= Camera.main;
    }
    private Vector2 _latestTravelPosition;
    private const float MinimumMagnitudeToMove = 0.05f;

    private PlayerParticleWeapon _currentWeapon;

    private BusEventBinding<EnemySpawnEventWrapper> _enemySpawnEventBinding;

    [SerializeField][Range(0, 10)] private float _baseMoveSpeed;
    public float BaseMoveSpeed
    {
        get => _baseMoveSpeed;
        set => _baseMoveSpeed = value;
    }

    protected override void Awake()
    {
        base.Awake();
        _enemySpawnEventBinding = new BusEventBinding<EnemySpawnEventWrapper>(RegisterPlayerForEnemies);
        _mainCamera = Camera.main;
        OnDestroyEvent += () => EventBus<PlayerDeathEventWrapper>.Raise(new PlayerDeathEventWrapper());
    }

    private void OnEnable()
    {
        EventBus<EnemySpawnEventWrapper>.Register(_enemySpawnEventBinding);
    }

    private void OnDisable()
    {
        EventBus<EnemySpawnEventWrapper>.Deregister(_enemySpawnEventBinding);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            MovePlayerTowardsInput();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThisRigidbody.WakeUp();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ThisRigidbody.Sleep();
        }

        if (Input.GetMouseButton(0))
        {
            _latestTravelPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void RegisterPlayerForEnemies(EnemySpawnEventWrapper enemySpawnEventWrapper)
    {
        foreach (EnemyParticleWeapon weapon in enemySpawnEventWrapper.EnemyParticleWeapons)
        {
            weapon.AddCollider(GetComponent<Collider2D>());
        }
    }

    private void MovePlayerTowardsInput()
    {
        Vector2 playerForwardVector = transform.up;
        Vector2 playerToMouseDistance = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        Vector2 playerToMouseDir = playerToMouseDistance.normalized;

        if (playerToMouseDistance.sqrMagnitude > MinimumMagnitudeToMove)
        {
            //transform.position += (Vector3)playerToMouseDir * CurrentMoveSpeed;
            ThisRigidbody.position += (playerToMouseDir * BaseMoveSpeed) * Time.fixedDeltaTime;
        }
        transform.Rotate(Vector3.forward, Vector2.SignedAngle(playerForwardVector, playerToMouseDir), Space.World);
        //_rigidbody2D.SetRotation(Vector2.SignedAngle(playerForwardVector, playerToMouseDir));
        //_rigidbody2D.MoveRotation(Vector2.SignedAngle(playerForwardVector, playerToMouseDir));
    }

    public override void ReceiveDamage(float damage)
    {
        if (_flashDamageCoroutine == null) _flashDamageCoroutine = StartCoroutine(FlashDamage(3));
        this.CurrentHealth -= damage;
        RaiseHealthEvent();
    }

    public override void AddHealth(float hpToAdd)
    {
        this.CurrentHealth += hpToAdd;
        RaiseHealthEvent();
    }

    private void RaiseHealthEvent()
    {
        EventBus<PlayerHealthEventWrapper>.Raise(new PlayerHealthEventWrapper
        {
            CurrentHealth = this.CurrentHealth,
            MaxHealth = this.MaxHealth
        }
        );
    }

    public void SetCurrentWeapon(PlayerParticleWeapon weapon)
    {
        _currentWeapon = weapon;
    }

    #region FLASHINGDAMAGE
    private const float ColorFlashDecrementValue = 17.0f / 255.0f;
    private Coroutine _flashDamageCoroutine;

    private IEnumerator FlashDamage(int numberOfRepeats)
    {
        Color color = Color.red;
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        while (color.r > 0)
        {
            color.r -= ColorFlashDecrementValue;
            CharacterRenderer.color = color;
            if (color.r < 0) color.r = 0;
            yield return delay;
        }

        while (color.r < 1)
        {
            color.r += ColorFlashDecrementValue;
            CharacterRenderer.color = color;
            if (color.r > 1) color.r = 1;
            yield return delay;
        }

        if (numberOfRepeats != 0) yield return StartCoroutine(FlashDamage(numberOfRepeats - 1));
        else
        {
            CharacterRenderer.color = Color.white;
            _flashDamageCoroutine = null;
        }
    }
    #endregion
}
