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
    public ExplosionOnDestroy GetNewExplosion(ProjectileData explosionVisuals)
    {
        ExplosionOnDestroy explosion = GetObjectFromPool();
        explosion.SetExplosionVisuals(explosionVisuals);
        explosion.gameObject.SetActive(true);
        return explosion;
    }

    public void AddUnusedExplosion(ExplosionOnDestroy explosionOnDestroy)
    {
        explosionOnDestroy.gameObject.SetActive(false);
        AddObjectToPool(explosionOnDestroy);
    }

    protected override void CreateObjectIfEmpty()
    {
        ExplosionOnDestroy explosion = Instantiate(_explosionPrefab);
        AddUnusedExplosion(explosion);
    }
}
