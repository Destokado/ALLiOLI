using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUp : MatchPhase
{
    private float countdownTimer { get; set; }
    
    public override void StartPhase()
    {
        countdownTimer = 2 * 60;
        Debug.Log("STAGE 1 - Starting phase 'TrapUp'. The 1st stage just started. " + countdownTimer + "s remaining.");
    }

    public override void UpdatePhase(float deltaTime)
    {
        countdownTimer -= deltaTime;
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (countdownTimer > 0)
            return this;
        return new FinishingTrapUp();
    }

    public override void EndPhase()
    {
        
    }
}
