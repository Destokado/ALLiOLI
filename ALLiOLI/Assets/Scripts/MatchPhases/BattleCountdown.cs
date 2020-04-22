using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCountdown : State
{
    public override void StartState()
    {
        Debug.Log("STAGE 2 - Starting phase 'BattleCountdown'. The battle will start in 10s.");

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
        return new Battle();
    }

    public override void EndState()
    {
        
    }
}
