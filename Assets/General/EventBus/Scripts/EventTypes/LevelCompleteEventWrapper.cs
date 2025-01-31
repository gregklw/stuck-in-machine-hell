using UnityEngine.ResourceManagement.ResourceProviders;

public struct LevelCompleteEventWrapper : IEventWrapper
{
    public LevelSceneGroup CurrentLevelGroup;
    public SceneInstance BaseSceneInstance;
    //public SceneInstance SubLevelSceneInstance;
}
