using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUp : MatchPhase
{
    public override string informativeText { get => "Setup your traps!"; protected set {} }
    public override bool showTrapsCounter { get => true; protected set {} }
    public override bool showReadiness { get => true; protected set {} }
    public override bool showMatchTimer { get => true; protected set {} }
    
    public override void StartState()
    {
        MatchManager.Instance.matchTimer = 30;
        Debug.Log("STAGE 1 - Starting phase 'TrapUp'. The 1st stage just started. " + MatchManager.Instance.matchTimer + "s remaining.");
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchTimer > 0)
            MatchManager.Instance.matchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.matchTimer > 0 || !MatchManager.Instance.IsAnyPlayerReady())
            return this;

        return new FinishingTrapUp();
    }

    public override void EndState()
    {
        
    }
}
