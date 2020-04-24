using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MatchPhase
{
    public override void StartState()
    {
        Debug.Log("STAGE 2 - Starting phase 'Battle'. The battle just started.");
    }

    public override void UpdateState(float deltaTime)
    {
        
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.winnerPlayer==null)
            return this;
        return new End();
    }

    public override void EndState()
    {
        
    }
}
