using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class Cheats
{
    [MenuItem("ALLiOLI/Cheats/Own all traps")]
    public static void OwnAllTraps()
    {
        if (Application.isPlaying)
        {
            Trap[] allTraps = Object.FindObjectsOfType<Trap>();

            foreach (Client client in GameManager.singleton.clients)
                foreach (Player player in client.PlayersManager.players)
                {
                    foreach (Trap trap in allTraps)
                        player.ownedTraps.Add(trap);
                    Debug.Log("Player " + player.name + " owns " + player.ownedTraps.Count + " traps.");
                }
        }
        else
        {
            Debug.LogError("Not in play mode.");
        }
    }

    #region Forced phases

    [MenuItem("ALLiOLI/Set phase/0 - WaitingForPlayers")]
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

    [MenuItem("ALLiOLI/Set phase/1 - StartCountdown")]
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

    [MenuItem("ALLiOLI/Set phase/2 - TrapUp")]
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

    [MenuItem("ALLiOLI/Set phase/3 - FinishingTrapUp")]
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

    [MenuItem("ALLiOLI/Set phase/4 - BattleCountdown")]
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

    [MenuItem("ALLiOLI/Set phase/5 - Battle")]
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

    [MenuItem("ALLiOLI/Set phase/6 - End")]
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
}
#endif