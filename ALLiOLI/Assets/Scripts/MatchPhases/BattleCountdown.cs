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

    public override void StartState()
    {
        MatchManager.Instance.matchTimer = 10;
        Debug.Log("STAGE 2 - Starting phase 'BattleCountdown'. The battle will start in " +
                  MatchManager.Instance.matchTimer + "s.");
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchTimer > 0)
            MatchManager.Instance.matchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.matchTimer > 0)
            return this;
        return new Battle();
    }

    public override void EndState()
    {
    }
}