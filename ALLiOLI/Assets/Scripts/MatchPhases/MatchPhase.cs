using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatchPhase : State
{
    public abstract bool showTrapsCounter { get; protected set; }
    public abstract bool showReadiness { get; protected set; }
    public abstract bool showMatchTimer { get; protected set; }
}
