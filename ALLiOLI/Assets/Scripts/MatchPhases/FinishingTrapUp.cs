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
        MatchManager.instance.MatchTimer = 5;
        Debug.Log("STAGE 1 - Starting phase 'FinishingTrapUp'. The 1st stage will end in " +
                  MatchManager.instance.MatchTimer + "s.");
    }
    
    public override void ServerStartState() {}

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.instance.MatchTimer > 0)
            MatchManager.instance.MatchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.instance.MatchTimer > 0)
            return this;
        return new BattleCountdown();
    }

    public override void EndState()
    {
    }
}