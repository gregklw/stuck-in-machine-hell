using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackingCharacter : DestroyableObject
{
    [SerializeField][Range(0, 800)] private int _baseAttackSpeed = 480;

    private Rigidbody2D _rigidbody2D;
    protected Rigidbody2D ThisRigidbody => _rigidbody2D;

    public int BaseAttackSpeed
    {
        get => _baseAttackSpeed;
        set => _baseAttackSpeed = value;
    }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
