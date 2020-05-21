using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    public override string informativeText
    {
        get => "Press any button to join.\nWaiting for everyone to be ready.";
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
        MatchManager.instance.MatchTimer = 1;
        Client.localClient.PlayersManager.playerInputManager.enabled = true;
        GameManager.Instance.GUI.SetStartMatchConfiguration();
    }
    
    public override void ServerStartState() {}

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.instance.MatchTimer > 0)
            MatchManager.instance.MatchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.TotalCurrentPlayers <= 0 || MatchManager.instance.MatchTimer > 0)
            return this;
        
        if ( MatchManager.instance.AreAllPlayersReady())
            return new StartCountdown();

        return this;
    }

    public override void EndState()
    {
        
    }
}