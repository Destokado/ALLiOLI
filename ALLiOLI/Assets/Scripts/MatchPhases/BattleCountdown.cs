using UnityEngine;

public class BattleCountdown : MatchPhase
{
    public override string informativeText
    {
        get => "The battle will start soon";
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
        get => true; 
        protected set{}
    }

    public override void StartState()
    {
        base.StartState();
        MatchManager.instance.matchTimer = 3;
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.instance.matchTimer > 0)
            MatchManager.instance.matchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.instance.matchTimer > 0)
            return this;
        return new Battle();
    }
}