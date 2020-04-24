using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCountdown : MatchPhase
{
    public override void StartState()
    {
        MatchManager.Instance.countdownTimer = 10;
        Debug.Log("STAGE 2 - Starting phase 'BattleCountdown'. The battle will start in " + MatchManager.Instance.countdownTimer + "s.");
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
        return new Battle();
    }

    public override void EndState()
    {
        
    }
}
