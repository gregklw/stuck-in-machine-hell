using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class AssetReferenceAnimationController : AssetReferenceT<RuntimeAnimatorController>
{
    public AssetReferenceAnimationController(string guid)
        : base(guid)
    {
    }
}
