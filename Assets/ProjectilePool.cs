using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : SimpleObjectPool<GameObject>
{
    [SerializeField] private GameObject _projectileBasePrefab;

    public static ProjectilePool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetNewProjectile()
    {
        GameObject projectileBase = GetObjectFromPool();
        projectileBase.SetActive(true);
        return projectileBase;
    }

    public void AddUnusedProjectile(GameObject projectileBase)
    {
        projectileBase.SetActive(false);
        AddObjectToPool(projectileBase);
    }

    protected override void CreateObjectIfEmpty()
    {
        GameObject projectileBase = Instantiate(_projectileBasePrefab);
        AddObjectToPool(projectileBase);
    }
}
