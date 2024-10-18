using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : AttackingCharacter
{
    private Rigidbody2D _rigidbody2D;
    private Camera _mainCamera;
    private Camera MainCamera
    {
        get => _mainCamera ??= Camera.main;
    }
    //private Vector2 _currentSmoothedPosition;
    private Vector2 _latestTravelPosition;
    //private const float SpeedSmoothTime = 0.3f;
    private const float MinimumMagnitudeToMove = 0.05f;

    private float _counter;

    [SerializeField] private Transform _firingPivot;

    [SerializeField][Range(0, 10)] private float _baseMoveSpeed;
    public float BaseMoveSpeed
    {
        get => _baseMoveSpeed;
        set => _baseMoveSpeed = value;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            MovePlayerTowardsInput();
        }
        FireWeapon();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _latestTravelPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
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
            _rigidbody2D.position += (playerToMouseDir * BaseMoveSpeed) * Time.fixedDeltaTime;
        }
        transform.Rotate(Vector3.forward, Vector2.SignedAngle(playerForwardVector, playerToMouseDir), Space.World);
    }

    private void FireWeapon()
    {
        _counter += Time.fixedDeltaTime;
        if (_counter > CommonCalculations.CalculateAttackSpeed(BaseAttackSpeed * CurrentWeapon.AttackSpeedFactor))
        {
            CurrentWeapon.Fire(_firingPivot.transform.position, transform.up);
            _counter = 0;
        }
    }

    public override void ReceiveDamage(float damage)
    {
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
}
