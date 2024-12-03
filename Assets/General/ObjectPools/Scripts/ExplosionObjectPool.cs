using System;
using UnityEngine;

public class ExplosionObjectPool : SimpleObjectPool<ExplosionObject>
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

    [SerializeField] private ExplosionObject _explosionPrefab;
    public ExplosionObject GetNewExplosion(Vector2 spawnPoint, RuntimeAnimatorController animationController)
    {
        ExplosionObject explosion = GetObjectFromPool();
        explosion.SetExplosionVisuals(animationController);
        //explosion.gameObject.SetActive(true);
        explosion.transform.position = spawnPoint;
        return explosion;
    }

    public void AddUnusedExplosion(ExplosionObject explosionOnDestroy)
    {
        //explosionOnDestroy.gameObject.SetActive(false);
        explosionOnDestroy.transform.position = transform.position;
        AddObjectToPool(explosionOnDestroy);
    }

    protected override void CreateObjectIfEmpty()
    {
        ExplosionObject explosion = Instantiate(_explosionPrefab);
        AddUnusedExplosion(explosion);
    }
}
