using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponInfo", menuName = "ScriptableObjects/WeaponInfo")]
public class WeaponAttributes : ScriptableObject
{
    [SerializeField][Range(0, 2)] public float AttackSpeedFactor;
    public float Damage;
    public Projectile ProjectilePrefab;
}
