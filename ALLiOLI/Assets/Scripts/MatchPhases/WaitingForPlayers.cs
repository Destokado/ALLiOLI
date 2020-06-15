using System;
using UnityEngine;

public class WaitingForPlayers : MatchPhase
{
    public override string informativeText
    {
        get => "Press any button to join.\nWaiting for everyone to be ready.";
        protected set { }
    }

    public override bool allowMovementAndCameraRotation
    {
        get => true;
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
        MatchManager.instance.matchTimer = 1; // Securing timing, can not end instantly
    }
    

    public override void UpdateState(float deltaTime)
    {
        if (MatchManager.instance.matchTimer > 0)
            MatchManager.instance.matchTimer -= deltaTime;
    }

    public override State GetCurrentState()
    {
        if (MatchManager.TotalCurrentPlayers <= 0 || MatchManager.instance.matchTimer > 0)
            return this;
        
        if ( MatchManager.instance.AreAllPlayersReady())
            return new BattleCountdown();

        return this;
    }
}