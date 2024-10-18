using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelSceneGroup", menuName = "ScriptableObjects/LevelSceneGroup")]
public class LevelSceneGroup : ScriptableObject
{
    public SceneField StartScene;
    public SceneField[] LevelScenes;
    public LevelSceneGroup NextLevel;
}
