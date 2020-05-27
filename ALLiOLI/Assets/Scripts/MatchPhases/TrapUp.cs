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
        //get => true;
        get => false;
        protected set { }
    }

    public override bool showMatchTimer
    {
        //get => true;
        get => false;
        protected set { }
    }

    public override bool inGamingMode
    {
        get => true; 
        protected set{}
    }

    public override void StartState()
    {
        base.StartState();
        //MatchManager.Instance.MatchTimer = 30;
        Cheats.OwnAllTraps();
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.MatchTimer > 0)
            MatchManager.Instance.MatchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        //if (MatchManager.Instance.MatchTimer > 0 || !MatchManager.Instance.AreAllPlayersReady())
        //    return this;

        return new FinishingTrapUp();
    }
}