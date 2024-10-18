using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //consider optimizing using scriptableobject WeaponInfo reference
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeedFactor;
    public float Damage { get => _damage; set => _damage = value; }
    public float AttackSpeedFactor => _attackSpeedFactor;

    public abstract void Fire(Vector3 spawnPos, Vector3 facingDirection);
    protected Projectile InstantiateProjectile(Projectile projectilePrefab, Vector3 spawnPos, Vector3 facingDirection)
    {
        Projectile proj = MonoBehaviour.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        proj.Init(facingDirection, Damage);
        return proj;
    }
}