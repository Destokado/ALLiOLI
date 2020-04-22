using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    public override void StartPhase()
    {
        Debug.Log("Starting phase 'WaitingForPlayers'.");

    }

    public override void UpdatePhase(float deltaTime)
    {
        
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (MatchManager.Instance.playerInputManager.joiningEnabled)
            return this;
        else
            return new StartCountdown();
    }

    public override void EndPhase()
    {
        
    }
}
