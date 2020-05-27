using UnityEngine;

public class StartCountdown : MatchPhase
{
    public override string informativeText
    {
        get => "Starting the match in a few seconds";
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
        get => true;
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
        MatchManager.Instance.MatchTimer = 10;
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.MatchTimer > 0)
            MatchManager.Instance.MatchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.MatchTimer > 0)
            return this;
        return new TrapUp();
    }
}