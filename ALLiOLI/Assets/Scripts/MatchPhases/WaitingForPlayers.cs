using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    public override void StartPhase()
    {
        Debug.Log("STAGE 0 - Starting phase 'WaitingForPlayers'.");

    }

    public override void UpdatePhase(float deltaTime)
    {
        
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (MatchManager.Instance.playerInputManager.playerCount < MatchManager.Instance.playerInputManager.maxPlayerCount)
            return this;
        else
            return new StartCountdown();
    }

    public override void EndPhase()
    {
        
    }
}
