using System.Diagnostics;

public struct PlayerDeathEventWrapper : IEventWrapper
{
    public void SetGameOver(IObjective objective)
    {
        UnityEngine.Debug.Log("TRIGGERED PLAYER DEATH");
        objective.IsGameOver = true;
    }
}
