using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class Cheats
{
    
    #region Forced phases

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/-1 - None")]
    public static void SetPhaseNull()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(null);
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }
    
    [MenuItem("ALLiOLI/Cheats/Server/Set phase/0 - WaitingForPlayers")]
    public static void SetPhaseWaitingForPlayers()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(new WaitingForPlayers());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/1 - BattleCountdown")]
    public static void SetPhaseStartCountdown()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(new BattleCountdown());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/2 - Battle")]
    public static void SetPhaseTrapUp()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(new Battle());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/3 - EndRound")]
    public static void SetPhaseFinishingTrapUp()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(new EndRound());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/4 - EndMatch")]
    public static void SetPhaseBattleCountdown()
    {
        if (Application.isPlaying)
        {
            MatchManager.instance.BroadcastNewMatchPhase(new EndMatch());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    #endregion
    
    [MenuItem("ALLiOLI/Cheats/Server/Activate all traps")]
    public static void ActivateAllTraps()
    {
        if (Application.isPlaying)
        {
            Trap[] allTraps = Object.FindObjectsOfType<Trap>();
            foreach (Trap trap in allTraps)
                trap.Activate();
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }
    
        
    [MenuItem("ALLiOLI/Cheats/Server/Trap Activators/Give 100 to all players")]
    public static void Give100TrapActivatorsAllPlayers()
    {
        if (Application.isPlaying)
        {
            foreach (Client client in MatchManager.instance.clients)
                foreach (Player player in client.PlayersManager.players)
                    player.trapActivators += 100;
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }
    
    [MenuItem("ALLiOLI/Cheats/Server/Trap Activators/Give 100 to local players")]
    public static void Give100TrapActivatorsLocalPlayers()
    {
        if (Application.isPlaying)
        {
            foreach (Player player in Client.LocalClient.PlayersManager.players)
                player.trapActivators += 100;
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }
    
}
#endif