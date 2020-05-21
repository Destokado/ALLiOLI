using UnityEngine;

public class TrapUp : MatchPhase
{
    public override string informativeText
    {
        get => "Setup your traps!";
        protected set { }
    }

    public override bool showTrapsCounter
    {
        get => true;
        protected set { }
    }

    public override bool showReadiness
    {
        get => true;
        protected set { }
    }

    public override bool showMatchTimer
    {
        get => true;
        protected set { }
    }

    public override void StartState()
    {
        MatchManager.instance.MatchTimer = 30;
        //MatchManager.Instance.KillActiveCharacters();
        Debug.Log("STAGE 1 - Starting phase 'TrapUp'. The 1st stage just started. " + MatchManager.instance.MatchTimer +
                  "s remaining.");
    }
    
    public override void ServerStartState() {}

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.instance.MatchTimer > 0)
            MatchManager.instance.MatchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.instance.MatchTimer > 0 || !MatchManager.instance.AreAllPlayersReady())
            return this;

        return new FinishingTrapUp();
    }

    public override void EndState()
    {
    }
}