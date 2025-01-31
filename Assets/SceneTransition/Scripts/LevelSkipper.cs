using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkipper : MonoBehaviour
{
    public void SkipLevel()
    {
        EventBus<SubLevelProgressionEventWrapper>.Raise(new SubLevelProgressionEventWrapper());
    }
}
