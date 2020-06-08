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
        FlagSpawner.Instance.ResetFlags();
        MatchManager.instance.newRoundWinnerPlayerNetIdEvent += UpdateInformativeText;
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
        return $"The winner is {MatchManager.instance.roundWinnerName}!";
    }

    public override State GetCurrentState()
    {
        if (MatchManager.instance.AreAllPlayersReady())
            return new BattleCountdown();
        
        return this;
    }
    
}