using UnityEngine;

public class SineBullet : Projectile
{
    private Vector2 _spawnPosition;
    private float _counter;

    private void Awake()
    {
        _spawnPosition = transform.position;
    }
    public override void Move()
    {
        _counter += Time.fixedDeltaTime;
        _spawnPosition += (Vector2)transform.up;
        //Debug.Log($"{Mathf.Sin(_spawnPosition.x)}");
        //Debug.Log($"{transform.eulerAngles.z}/{Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)}");
        float angle = (transform.eulerAngles.z) * Mathf.Deg2Rad;
        float xSineOffset = Mathf.Sin(angle + Mathf.Sin(_counter * 30));
        float ySineOffset = Mathf.Cos(angle + Mathf.Sin(_counter * 30));
        //Debug.Log($"{xSineOffset}/{ySineOffset}");
        //float y = Mathf.Sin(_counter);

        //float xSineOffset = _counter * Mathf.Cos(angle) - y * Mathf.Sin(angle);
        //float ySineOffset = _counter * Mathf.Sin(angle) + y * Mathf.Cos(angle);

        Vector2 modifiedDir = transform.up;
        modifiedDir.x -= xSineOffset;
        modifiedDir.y += ySineOffset;

        transform.position += (Vector3) modifiedDir * ProjectileSpeed * Time.fixedDeltaTime;
    }

    public override void OnHitCollision(Collider2D collision)
    {
        //Destroy(gameObject);
        RemoveAndCacheProjectile();
    }
}
