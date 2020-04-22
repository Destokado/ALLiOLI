using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : State
{
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
