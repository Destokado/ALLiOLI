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
            case 1: return new BattleCountdown();
            case 2: return new Battle();
            case 3: return new EndRound();
            case 4: return new EndMatch();
        }

        return null;
    }

    public static int GetPhaseId(MatchPhase phase)
    {
        switch (phase)
        {
            case WaitingForPlayers p: return 0;
            case BattleCountdown p: return 1;
            case Battle p: return 2;
            case EndRound p: return 3;
            case EndMatch p: return 4;

            default: return -1;
        }
    }
}
