using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MatchPhase
{
    public override void StartPhase()
    {
        Debug.Log("Starting phase 'Battle'. The battle will start in 10s.");
    }

    public override void UpdatePhase(float deltaTime)
    {
        
    }

    public override MatchPhase GetCurrentPhase()
    {
        if (MatchManager.Instance.winnerPlayer==null)
            return this;
        return new End();
    }

    public override void EndPhase()
    {
        
    }
}
