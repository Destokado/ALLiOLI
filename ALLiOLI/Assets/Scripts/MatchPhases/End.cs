using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MatchPhase
{
    public override bool showTrapsCounter { get => false; protected set {} }
    public override bool showReadiness { get => false; protected set {} }
    public override bool showMatchTimer { get => false; protected set {} }
    
    public override void StartState()
    {
        Debug.Log("STAGE 3 - Starting phase 'End'. The game has finished");
    }

    public override void UpdateState(float deltaTime)
    {
        
    }

    public override State GetCurrentState()
    {
        return this;
    }

    public override void EndState()
    {
        
    }
}
