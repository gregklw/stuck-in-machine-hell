using UnityEngine;

public class SineBullet : IProjectileBehaviour
{
    private Vector2 _spawnPosition;
    private float _counter;

    public SineBullet(Vector2 spawnPosition)
    { 
        _spawnPosition = spawnPosition;
    }
    public void Move(Transform projectileTransform, float projectileSpeed)
    {
        _counter += Time.fixedDeltaTime;
        _spawnPosition += (Vector2)projectileTransform.up;
        //Debug.Log($"{Mathf.Sin(_spawnPosition.x)}");
        //Debug.Log($"{transform.eulerAngles.z}/{Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)}");
        float angle = (projectileTransform.eulerAngles.z) * Mathf.Deg2Rad;
        float xSineOffset = Mathf.Sin(angle + Mathf.Sin(_counter * 30));
        float ySineOffset = Mathf.Cos(angle + Mathf.Sin(_counter * 30));
        //Debug.Log($"{xSineOffset}/{ySineOffset}");
        //float y = Mathf.Sin(_counter);

        //float xSineOffset = _counter * Mathf.Cos(angle) - y * Mathf.Sin(angle);
        //float ySineOffset = _counter * Mathf.Sin(angle) + y * Mathf.Cos(angle);

        Vector2 modifiedDir = projectileTransform.up;
        modifiedDir.x -= xSineOffset;
        modifiedDir.y += ySineOffset;

        projectileTransform.position += (Vector3) modifiedDir * projectileSpeed * Time.fixedDeltaTime;
    }

    public void OnHitCollision()
    {
    }
}
