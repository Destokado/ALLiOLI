using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MatchPhase
{
    public override void StartPhase()
    {
        Debug.Log("STAGE 3 - Starting phase 'End'. The game has finished");
    }

    public override void UpdatePhase(float deltaTime)
    {
        
    }

    public override MatchPhase GetCurrentPhase()
    {
        return this;
    }

    public override void EndPhase()
    {
        
    }
}
