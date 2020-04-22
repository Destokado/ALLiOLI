using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingTrapUp : State
{
    public override void StartState()
    {
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in 10s.");

        MatchManager.Instance.countdownTimer = 10;
    }

    public override void UpdateState(float deltaTime)
    {
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
