using Mirror;
using UnityEngine;

public class End : MatchPhase
{
    public override string informativeText
    {
        get => "";
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
    
    public override void StartState()
    {
        Debug.Log("STAGE 3 - Starting phase 'End'. The game has finished.\nThe winner is " + MatchManager.instance.WinnerPlayerNetId);
        MatchManager.instance.guiManager.UpdateEndScreen(true);
    }
    
    public override void ServerStartState() {}

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