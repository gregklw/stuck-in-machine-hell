using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackingCharacter : DestroyableObject
{
    [SerializeField][Range(0, 800)] private int _baseAttackSpeed = 480;

    public int BaseAttackSpeed
    {
        get => _baseAttackSpeed;
        set => _baseAttackSpeed = value;
    }
}
