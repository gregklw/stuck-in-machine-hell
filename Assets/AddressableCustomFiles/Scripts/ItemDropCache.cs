using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemDropCache : MonoBehaviour
{
    private Dictionary<AssetReferenceGameObject, GameObject> _itemDropCache = new Dictionary<AssetReferenceGameObject, GameObject>();

    public void CacheLoadedDrop(AssetReferenceGameObject assetReference, GameObject loadedGameObject)
    { 
        if (!ContainsItem(assetReference)) _itemDropCache.Add(assetReference, loadedGameObject);
    }

    public GameObject GetCachedItemDrop(AssetReferenceGameObject assetReference)
    { 
        return _itemDropCache[assetReference];
    }

    public bool ContainsItem(AssetReferenceGameObject assetReference)
    { 
        return _itemDropCache.ContainsKey(assetReference);
    }
}
