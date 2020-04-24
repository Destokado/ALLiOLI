using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MatchPhase
{
    public override void StartState()
    {
        MatchManager.Instance.countdownTimer = 10;
        Debug.Log("STAGE 0 - Starting phase 'StartCountdown'. The 1st stage will start in " + MatchManager.Instance.countdownTimer + "s.");
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
        return new TrapUp();
    }

    public override void EndState()
    {
        
    }
}
