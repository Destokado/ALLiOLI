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

    public override void StartState()
    {
        MatchManager.Instance.matchTimer = 10;
        Debug.Log("STAGE 0 - Starting phase 'StartCountdown'. The 1st stage will start in " +
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
        return new TrapUp();
    }

    public override void EndState()
    {
    }
}