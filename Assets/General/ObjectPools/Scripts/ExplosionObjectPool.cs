using System;
using UnityEngine;

public class ExplosionObjectPool : SimpleObjectPool<ExplosionOnDestroy>
{
    public static ExplosionObjectPool Instance;
    [SerializeField] private int _numberOfPrefabsToInstantiate;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < _numberOfPrefabsToInstantiate; i++)
        {
            CreateObjectIfEmpty();
        }
    }

    [SerializeField] private ExplosionOnDestroy _explosionPrefab;
    public ExplosionOnDestroy GetNewExplosion(Vector2 spawnPoint, ProjectileData explosionVisuals)
    {
        ExplosionOnDestroy explosion = GetObjectFromPool();
        explosion.SetExplosionVisuals(explosionVisuals);
        //explosion.gameObject.SetActive(true);
        explosion.transform.position = spawnPoint;
        return explosion;
    }

    public void AddUnusedExplosion(ExplosionOnDestroy explosionOnDestroy)
    {
        //explosionOnDestroy.gameObject.SetActive(false);
        explosionOnDestroy.transform.position = transform.position;
        AddObjectToPool(explosionOnDestroy);
    }

    protected override void CreateObjectIfEmpty()
    {
        ExplosionOnDestroy explosion = Instantiate(_explosionPrefab);
        AddUnusedExplosion(explosion);
    }
}
