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

    public override void StartState()
    {
        MatchManager.Instance.matchTimer = 5;
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in " +
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
        return new BattleCountdown();
    }

    public override void EndState()
    {
    }
}