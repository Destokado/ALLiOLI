using Mirror;
using UnityEngine;

public class EndMatch : MatchPhase
{
    public override string informativeText
    {
        get => "";
        protected set { }
    }

    public override bool allowMovement
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
        get => false; 
        protected set{}
    }

    public override void StartState()
    {
        base.StartState();
        MatchManager.instance.guiManager.UpdateEndScreen(true);
    }

    public override State GetCurrentState()
    {
        return this;
    }
    
}