using UnityEngine;

public class Battle : MatchPhase
{
    public override string informativeText
    {
        get => "Bring the flag to the spawn point";
        protected set { }
    }

    public override bool showTrapsCounter
    {
        get => false;
        protected set { }
    }

    public override bool showReadiness
    {
        get => false;
        protected set { }
    }

    public override bool showMatchTimer
    {
        get => false;
        protected set { }
    }

    public override bool inGamingMode
    {
        get => true; 
        protected set{}
    }
    
    public override void ServerStartState()
    {
        base.ServerStartState();
        FlagSpawner.Instance.Spawn();
    }

    public override State GetCurrentState()
    {
        if (!MatchManager.Instance.ThereIsWinner)
            return this;
        
        return new End();
    }
    
}