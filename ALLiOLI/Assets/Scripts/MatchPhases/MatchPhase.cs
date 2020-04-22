using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchPhase
{
    public abstract void StartPhase();
    public abstract void UpdatePhase(float deltaTime);
    public abstract MatchPhase GetCurrentPhase(); //Returns the next Phase if this should be ended
    public abstract void EndPhase(); 
}
