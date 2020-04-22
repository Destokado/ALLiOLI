using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCountdown : MatchPhase
{
    private float countdownTimer { get; set; }
    
    public override void StartPhase()
    {
        Debug.Log("STAGE 2 - Starting phase 'BattleCountdown'. The battle will start in 10s.");

        countdownTimer = 10;
    }

    public override void UpdatePhase(float deltaTime)
    {
        countdownTimer -= deltaTime;
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (countdownTimer > 0)
            return this;
        return new Battle();
    }

    public override void EndPhase()
    {
        
    }
}
