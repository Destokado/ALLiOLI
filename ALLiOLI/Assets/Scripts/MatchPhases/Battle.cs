using UnityEngine;

public class Battle : MatchPhase
{
    private MatchPhase matchPhaseImplementation;

    public override string informativeText
    {
        get => "Bring the flag to the spawn point.";
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
        get => false;
        protected set { }
    }

    public override void StartState()
    {
        Debug.Log("STAGE 2 - Starting phase 'Battle'. The battle just started.");
        //MatchManager.Instance.KillActiveCharacters();
    }

    public override void ServerStartState()
    {
        FlagSpawner.Instance.Spawn();
    }

    public override void UpdateState(float deltaTime)
    {
    }

    public override State GetCurrentState()
    {
        if (!MatchManager.instance.ThereIsWinner)
            return this;
        
        return new End();
    }

    public override void EndState()
    {
    }
}