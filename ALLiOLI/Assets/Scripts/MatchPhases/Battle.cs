using UnityEngine;

public class Battle : MatchPhase
{
    public override string informativeText
    {
        get => "Bring the flag to the spawn point";
        protected set { }
    }

    public override bool allowMovementAndCameraRotation
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
        get => false;
        protected set { }
    }

    public override bool inGamingMode
    {
        get => true; 
        protected set{}
    }
    
    public override void ServerStartState()
    {
        base.ServerStartState();
        //FlagSpawner.Instance.ActivateFlags();
        MatchManager.instance.KillAllCharacters();
        MatchManager.instance.ResetWinner();
        foreach (Client client in MatchManager.instance.clients)
        {
            foreach (Player player in client.PlayersManager.players)
            {
                player.trapActivators = Player.startTrapActivators;
            }
        }
    }

    public override State GetCurrentState()
    {
        if (!MatchManager.instance.thereIsWinner)
            return this;
        
        return new EndRound();
    }
    
}