using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public struct DropInfo
{
    public int DropPercentage;
    //public GameObject DropPrefab;
    public AssetReferenceGameObject DropPrefabReference;
}
