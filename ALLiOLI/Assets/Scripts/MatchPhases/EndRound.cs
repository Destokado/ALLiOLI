using Mirror;
using UnityEngine;

public class EndRound : MatchPhase
{
    public override string informativeText
    {
        get => GetInformativeText();
        protected set { }
    }

    public override bool allowMovementAndCameraRotation
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

    public override bool inGamingMode
    {
        get => true; 
        protected set{}
    }

    public override void StartState()
    {
        base.StartState();
        MatchManager.instance.newRoundWinnerPlayerNetIdEvent += UpdateInformativeText;
    }

    public override void ServerStartState()
    {
        base.ServerStartState();
        FlagSpawner.Instance.ResetFlags();
    }

    public override void EndState()
    {
        base.EndState();
        // ReSharper disable once DelegateSubtraction
        MatchManager.instance.newRoundWinnerPlayerNetIdEvent -= UpdateInformativeText;
    }

    private void UpdateInformativeText()
    {
        MatchManager.instance.guiManager.UpdateInformativeText(GetInformativeText());
    }

    private string GetInformativeText()
    {
        string color = "#ffffff";
        string winnerString = $"<color={color}>The winner is </color>{MatchManager.instance.roundWinnerName}<color={color}>!</color>";
        return winnerString + "\nPress \"ready\" to play again" ;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.instance.AreAllPlayersReady())
            return new BattleCountdown();
        
        return this;
    }
    
}