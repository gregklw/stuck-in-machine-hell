using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "NewLevelSceneGroup", menuName = "ScriptableObjects/LevelSceneGroup")]
public class LevelSceneGroup : ScriptableObject
{
    public AssetReference StartScene;
    public AssetReference[] LevelScenes;
    public LevelSceneGroup NextLevel;
}
