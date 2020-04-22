using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingTrapUp : MatchPhase
{
    private float countdownTimer { get; set; }
    
    public override void StartPhase()
    {
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in 10s.");

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
        return new BattleCountdown();
    }

    public override void EndPhase()
    {
        
    }
}
