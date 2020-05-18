using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    public override string informativeText
    {
        get => "Waiting for all players to join and to be ready.";
        protected set { }
    }

    public override bool showTrapsCounter
    {
        get => false;
        protected set { }
    }

    public override bool showReadiness
    {
        get => true;
        protected set { }
    }

    public override bool showMatchTimer
    {
        get => false;
        protected set { }
    }

    public override void StartState()
    {
        Debug.Log("STAGE 0 - Starting phase 'WaitingForPlayers'.");
        MatchManager.Instance.matchTimer = 1;
        Client.localClient.PlayersManager.playerInputManager.enabled = true;
    }

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.Instance.matchTimer > 0)
            MatchManager.Instance.matchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (GameManager.TotalPlayers <= 0 || MatchManager.Instance.matchTimer > 0)
            return this;
        
        if ( GameManager.singleton.AreAllPlayersReady() || GameManager.TotalPlayers >= GameManager.maxPlayerCount)
            return new StartCountdown();

        return this;
    }

    public override void EndState()
    {
        
    }
}