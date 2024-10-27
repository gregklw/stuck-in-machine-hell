using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectilePool : SimpleObjectPool<Projectile>
{
    [SerializeField] private int _numberOfPrefabsToInstantiate;

    [SerializeField] private Projectile _projectileBasePrefab;

    public static ProjectilePool Instance;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < _numberOfPrefabsToInstantiate; i++)
        {
            CreateObjectIfEmpty();
        }
    }

    public Projectile GetNewProjectile()
    {
        Projectile projectileBase = GetObjectFromPool();
        projectileBase.gameObject.SetActive(true);
        return projectileBase;
    }

    public void CacheUnusedProjectile(Projectile projectileBase)
    {
        AddProjectileToPool(projectileBase);
    }

    protected override void CreateObjectIfEmpty()
    {
        Projectile projectileBase = Instantiate(_projectileBasePrefab);
        AddProjectileToPool(projectileBase);
    }

    private void AddProjectileToPool(Projectile projectileBase)
    {
        //PrefabUtility.RevertPrefabInstance(projectileBase, InteractionMode.AutomatedAction);
        //projectileBase.transform.localScale = Vector3.one;
        projectileBase.ResetProjectile();
        projectileBase.gameObject.SetActive(false);
        AddObjectToPool(projectileBase);
    }
}
