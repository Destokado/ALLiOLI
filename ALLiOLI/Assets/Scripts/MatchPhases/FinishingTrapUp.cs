using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingTrapUp : MatchPhase
{
    public override void StartState()
    {
        MatchManager.Instance.countdownTimer = 5;
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in " + MatchManager.Instance.countdownTimer + "s.");
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.countdownTimer > 0)
            MatchManager.Instance.countdownTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.countdownTimer > 0)
            return this;
        return new BattleCountdown();
    }

    public override void EndState()
    {
        
    }
}
