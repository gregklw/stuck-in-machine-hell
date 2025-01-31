using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDropInfoContainer", menuName = "ScriptableObjects/DropInfoContainer")]
public class DropInfoContainer : ScriptableObject
{
    public DropInfo[] DropInfoAllItems;
    public int DropProbabilityTotal;

#if UNITY_EDITOR
    private void OnValidate()
    {

        int dropProbabilityTotal = 0;
        foreach (DropInfo dropInfo in DropInfoAllItems)
        {
            dropProbabilityTotal += dropInfo.DropPercentage;
        }

        if (DropProbabilityTotal < dropProbabilityTotal)
            DropProbabilityTotal = dropProbabilityTotal;
    }
#endif
}
