using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatchPhaseManager
{
    public static MatchPhase GetNewMatchPhase(int phaseId)
    {
        switch (phaseId)
        {
            case 0: return new WaitingForPlayers();
            case 1: return new StartCountdown();
            case 2: return new TrapUp();
            case 3: return new FinishingTrapUp();
            case 4: return new BattleCountdown();
            case 5: return new Battle();
            case 6: return new End();
        }

        return null;
    }

    public static int GetPhaseId(MatchPhase phase)
    {
        switch (phase)
        {
            case WaitingForPlayers p: return 0;
            case StartCountdown p: return 1;
            case TrapUp p: return 2;
            case FinishingTrapUp p: return 3;
            case BattleCountdown p: return 4;
            case Battle p: return 5;
            case End p: return 6;
            
            default: return -1;
        }
    }
}
