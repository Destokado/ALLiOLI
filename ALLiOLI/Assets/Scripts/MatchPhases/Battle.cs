using UnityEngine;

public class Battle : MatchPhase
{
    public override string informativeText
    {
        get => "Bring the flag to the spawn point";
        protected set { }
    }

    public override bool allowMovement
    {
        get => true;
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
        FlagManager.Instance.Spawn();
        MatchManager.instance.KillAllCharacters();
        MatchManager.instance.ResetWinner();
    }

    public override State GetCurrentState()
    {
        if (!MatchManager.instance.thereIsWinner)
            return this;
        
        return new EndRound();
    }
    
}