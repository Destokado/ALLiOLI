using UnityEngine;

public class BattleCountdown : MatchPhase
{
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
        MatchManager.Instance.countdownTimer = 10;
        Debug.Log("STAGE 2 - Starting phase 'BattleCountdown'. The battle will start in " +
                  MatchManager.Instance.countdownTimer + "s.");
        FlagSpawner.Instance.Spawn();
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchCountdown > 0)
            MatchManager.Instance.matchCountdown -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.Instance.matchCountdown > 0)
            return this;
        return new Battle();
    }

    public override void EndState()
    {
    }
}