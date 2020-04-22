using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : State
{
    public override void StartState()
    {
        Debug.Log("STAGE 0 - Starting phase 'StartCountdown'. The 1st stage will start in 10s.");

        MatchManager.Instance.countdownTimer = 10;
    }

    public override void UpdateState(float deltaTime)
    {
        MatchManager.Instance.countdownTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.countdownTimer>0)
            return this;
        return new TrapUp();
    }

    public override void EndState()
    {
        
    }
}
