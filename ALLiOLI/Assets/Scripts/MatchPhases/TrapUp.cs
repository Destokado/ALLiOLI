using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUp : State
{
    public override void StartState()
    {
        MatchManager.Instance.countdownTimer = 2 * 60;
        Debug.Log("STAGE 1 - Starting phase 'TrapUp'. The 1st stage just started. " + MatchManager.Instance.countdownTimer + "s remaining.");
    }

    public override void UpdateState(float deltaTime)
    {
        MatchManager.Instance.countdownTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.countdownTimer > 0)
            return this;
        return new FinishingTrapUp();
    }

    public override void EndState()
    {
        
    }
}
