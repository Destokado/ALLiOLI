using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    
    public override bool showTrapsCounter { get => false; protected set {} }
    public override bool showReadiness { get => true; protected set {} }
    public override bool showMatchTimer { get => false; protected set {} }
    
    public override void StartState()
    {
        Debug.Log("STAGE 0 - Starting phase 'WaitingForPlayers'.");
    }

    public override void UpdateState(float deltaTime)
    {
        
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.AreAllPlayersReady() || MatchManager.Instance.playerInputManager.playerCount >= MatchManager.Instance.playerInputManager.maxPlayerCount)
            return new StartCountdown();
            
        return this;
    }

    public override void EndState()
    {
        
    }
}
