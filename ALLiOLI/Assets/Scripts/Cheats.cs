using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class Cheats
{
    [MenuItem("ALLiOLI/Cheats/Client/Own all traps")]
    public static void OwnAllTraps()
    {
        if (Application.isPlaying)
        {
            Trap[] allTraps = Object.FindObjectsOfType<Trap>();
            
            foreach (Player player in Client.localClient.PlayersManager.players)
            {
                if (player.HumanLocalPlayer == null)
                    continue;
                
                foreach (Trap trap in allTraps)
                    player.HumanLocalPlayer.ownedTraps.Add(trap);
                
                Debug.Log("Player '" + player.name + "' owns " + player.HumanLocalPlayer.ownedTraps.Count + " traps.");
            }
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    #region Forced phases

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/-1 - None")]
    public static void SetPhaseNull()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(null);
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
            MatchManager.Instance.BroadcastNewMatchPhase(new WaitingForPlayers());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/1 - StartCountdown")]
    public static void SetPhaseStartCountdown()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new StartCountdown());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/2 - TrapUp")]
    public static void SetPhaseTrapUp()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new TrapUp());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/3 - FinishingTrapUp")]
    public static void SetPhaseFinishingTrapUp()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new FinishingTrapUp());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/4 - BattleCountdown")]
    public static void SetPhaseBattleCountdown()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new BattleCountdown());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/5 - Battle")]
    public static void SetPhaseBattle()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new Battle());
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    [MenuItem("ALLiOLI/Cheats/Server/Set phase/6 - End")]
    public static void SetPhaseEnd()
    {
        if (Application.isPlaying)
        {
            MatchManager.Instance.BroadcastNewMatchPhase(new End());
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
}
#endif