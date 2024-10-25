using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectilePool : SimpleObjectPool<GameObject>
{
    [SerializeField] private int _numberOfPrefabsToInstantiate;

    [SerializeField] private GameObject _projectileBasePrefab;

    public static ProjectilePool Instance;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < _numberOfPrefabsToInstantiate; i++)
        {
            CreateObjectIfEmpty();
        }
    }

    public GameObject GetNewProjectile()
    {
        GameObject projectileBase = GetObjectFromPool();
        projectileBase.SetActive(true);
        return projectileBase;
    }

    public void CacheUnusedProjectile(GameObject projectileBase)
    {
        AddProjectileToPool(projectileBase);
    }

    protected override void CreateObjectIfEmpty()
    {
        GameObject projectileBase = Instantiate(_projectileBasePrefab);
        AddProjectileToPool(projectileBase);
    }

    private void AddProjectileToPool(GameObject projectileBase)
    {
        //PrefabUtility.RevertPrefabInstance(projectileBase, InteractionMode.AutomatedAction);
        //projectileBase.transform.localScale = Vector3.one;
        projectileBase.SetActive(false);
        AddObjectToPool(projectileBase);
    }
}
