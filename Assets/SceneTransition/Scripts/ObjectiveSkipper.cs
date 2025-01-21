using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSkipper : MonoBehaviour
{
    public void SkipObjectives()
    {
        EventBus<LevelProgressEventWrapper>.Raise(new LevelProgressEventWrapper());
    }
}
