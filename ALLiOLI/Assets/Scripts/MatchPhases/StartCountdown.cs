using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MatchPhase
{
    public override bool showTrapsCounter { get => false; protected set {} }
    public override bool showReadiness { get => false; protected set {} }
    public override bool showMatchTimer { get => true; protected set {} }
    
    public override void StartState()
    {
        MatchManager.Instance.matchCountdown = 10;
        Debug.Log("STAGE 0 - Starting phase 'StartCountdown'. The 1st stage will start in " + MatchManager.Instance.matchCountdown + "s.");
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchCountdown > 0)
            MatchManager.Instance.matchCountdown -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.matchCountdown > 0)
            return this;
        return new TrapUp();
    }

    public override void EndState()
    {
        
    }
}
