using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MatchPhase
{
    private float countdownTimer { get; set; }

    public override void StartPhase()
    {
        Debug.Log("Starting phase 'StartCountdown'. The 1st stage will start in 10s.");

        countdownTimer = 10;
    }

    public override void UpdatePhase(float deltaTime)
    {
        countdownTimer -= deltaTime;
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (countdownTimer>0)
            return this;
        return new TrapUp();
    }

    public override void EndPhase()
    {
        
    }
}
