using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkipper : MonoBehaviour
{
    public void SkipLevel()
    {
        EventBus<SubLevelProgressionEventWrapper>.Raise(new SubLevelProgressionEventWrapper());
    }

    public void DestroyObjects()
    {
        List<IAddressableUnloadable> unloadables = UnityUtils.FindInterfaces<IAddressableUnloadable>();
        unloadables.ForEach(unloadable => unloadable.Destroy());
    }

    public void ReleaseAssets()
    {
        List<IAddressableUnloadable> unloadables = UnityUtils.FindInterfaces<IAddressableUnloadable>();
        unloadables.ForEach(unloadable => unloadable.Unload());
    }
}
