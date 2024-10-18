using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//used for managing item drop events when holder is destroyed
public class ItemDropper : MonoBehaviour
{
    [SerializeField] private DropInfoContainer _dropInfoContainer;

    //need to replace in the future with OnDisable
    private void OnDisable()
    {
        SpawnItemByChance();
    }

    private void SpawnItemByChance()
    {
        if (_dropInfoContainer == null) return;

        int randomNumber = Random.Range(0, _dropInfoContainer.DropProbabilityTotal);
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
                Instantiate(dropInfo.DropPrefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }

    private bool IsNumberWithinRange(int value, int minInclusive, int maxInclusive)
    {
        return value >= minInclusive && value <= maxInclusive;
    }
}


