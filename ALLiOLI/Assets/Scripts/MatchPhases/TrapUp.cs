using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUp : MatchPhase
{
    private float countdownTimer { get; set; }
    
    public override void StartPhase()
    {
        Debug.Log("Starting phase 'TrapUp'. The 1st just started.");

        countdownTimer = 2 * 60;
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
