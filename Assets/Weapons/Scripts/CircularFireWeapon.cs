using UnityEngine;

public class CircularFireWeapon : Weapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField][Range(0, 20)] private int _numberOfShrapnel;
    public override void Fire(Vector3 spawnPos, Vector3 facingDirection)
    {
        float angle = 0;
        for (int i = 0; i < _numberOfShrapnel; i++)
        {
            angle += (360 / (float)_numberOfShrapnel) * i;
            Projectile shrapnel = InstantiateProjectile(projectilePrefab, transform.position, CommonCalculations.RotateTowardsUp(Vector3.up, angle));
        }
    }
}
