using UnityEngine;

public class FinishingTrapUp : MatchPhase
{
    public override string informativeText
    {
        get => "Final seconds to setup traps";
        protected set { }
    }

    public override bool showTrapsCounter
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
        MatchManager.Instance.MatchTimer = 3;
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
        return new BattleCountdown();
    }
}