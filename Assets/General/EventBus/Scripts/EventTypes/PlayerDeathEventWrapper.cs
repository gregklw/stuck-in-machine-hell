using System.Diagnostics;
using UnityEngine.ResourceManagement.ResourceProviders;

public struct PlayerDeathEventWrapper : IEventWrapper
{
    public void SetGameOver(IObjective objective)
    {
        UnityEngine.Debug.Log("TRIGGERED PLAYER DEATH");
        objective.IsGameOver = true;
    }

    public LevelSceneGroup CurrentLevelGroup;
    public SceneInstance BaseSceneInstance;
}
