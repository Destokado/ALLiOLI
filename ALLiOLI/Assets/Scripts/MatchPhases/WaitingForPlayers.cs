using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : State
{
    public override void StartState()
    {
        Debug.Log("STAGE 0 - Starting phase 'WaitingForPlayers'.");
    }

    public override void UpdateState(float deltaTime)
    {
        
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.playerInputManager.playerCount < MatchManager.Instance.playerInputManager.maxPlayerCount)
            return this;
        else
            return new StartCountdown();
    }

    public override void EndState()
    {
        
    }
}
