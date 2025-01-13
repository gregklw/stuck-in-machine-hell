using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//used for managing item drop events when holder is destroyed
public class ItemDropper : MonoBehaviour, IPostAddressableLoadable
{
    [SerializeField] private DropInfoContainer _dropInfoContainer;
    private ItemDropCache _dropCache;

    ////need to replace in the future with OnDisable
    //private void OnDisable()
    //{
    //    SpawnItemByChance();
    //}

    private void OnEnable()
    {
        _dropCache = FindObjectOfType<ItemDropCache>();
        StartCoroutine(CacheDropInfoPrefabs());
    }

    public void SpawnItemByChance()
    {
        if (_dropInfoContainer == null) return;

        int randomNumber = UnityEngine.Random.Range(0, _dropInfoContainer.DropProbabilityTotal);
        //forloop needs to be optimized by caching array to prevent unnecessary calling
        int floorValue = 0, ceilingValue = 0;

        foreach (var dropInfo in _dropInfoContainer.DropInfoAllItems)
        {
            //go through each drop info and see if any powerup drops
            //drop percentage needs to be optimized by not calling directly through array

            floorValue = ceilingValue;
            ceilingValue += dropInfo.DropPercentage;
            float dropChance = dropInfo.DropPercentage / (float) _dropInfoContainer.DropProbabilityTotal;
            //Debug.Log($"Floor value: {floorValue} | Ceiling value {ceilingValue} | Value: {randomNumber} | Within range: {IsNumberWithinRange(randomNumber, floorValue, ceilingValue)}");
            if (IsNumberWithinRange(randomNumber, floorValue, ceilingValue))
            {
                Instantiate(_dropCache.GetCachedItemDrop(dropInfo.DropPrefabReference), transform.position, Quaternion.identity);
                return;
            }
        }
    }

    private bool IsNumberWithinRange(int value, int minInclusive, int maxInclusive)
    {
        return value >= minInclusive && value <= maxInclusive;
    }
    public IEnumerator Init()
    {
        _dropCache ??= FindObjectOfType<ItemDropCache>();
        //Debug.Log($"Which GameObject: {gameObject.name}");
        yield return CacheDropInfoPrefabs();
    }

    private IEnumerator CacheDropInfoPrefabs()
    {
        if (_dropInfoContainer == null) yield break;

        foreach (var dropInfo in _dropInfoContainer.DropInfoAllItems)
        {
            if (_dropCache.ContainsItem(dropInfo.DropPrefabReference)) continue;
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(dropInfo.DropPrefabReference);
            yield return handle;
            _dropCache.CacheLoadedDrop(dropInfo.DropPrefabReference, handle.Result);
            //Debug.Log($"Loaded Item: {_dropCache.GetCachedItemDrop(dropInfo.DropPrefabReference)}");
        }
    }
}


