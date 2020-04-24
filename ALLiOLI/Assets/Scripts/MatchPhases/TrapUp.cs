using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUp : MatchPhase
{
    public override bool showTrapsCounter { get => true; protected set {} }
    public override bool showReadiness { get => true; protected set {} }
    public override bool showMatchTimer { get => true; protected set {} }
    
    public override void StartState()
    {
        MatchManager.Instance.matchCountdown = 30;
        Debug.Log("STAGE 1 - Starting phase 'TrapUp'. The 1st stage just started. " + MatchManager.Instance.matchCountdown + "s remaining.");
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchCountdown > 0)
            MatchManager.Instance.matchCountdown -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.matchCountdown > 0 || !MatchManager.Instance.IsAnyPlayerReady())
            return this;

        return new FinishingTrapUp();
    }

    public override void EndState()
    {
        
    }
}
