using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingTrapUp : MatchPhase
{
    public override bool showTrapsCounter { get => true; protected set {} }
    public override bool showReadiness { get => false; protected set {} }
    public override bool showMatchTimer { get => true; protected set {} }
    
    public override void StartState()
    {
        MatchManager.Instance.matchCountdown = 5;
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in " + MatchManager.Instance.matchCountdown + "s.");
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
        return new BattleCountdown();
    }

    public override void EndState()
    {
        
    }
}
