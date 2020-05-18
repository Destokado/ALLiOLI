using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : NetworkBehaviour
{
    private float _matchTimer;
    
    [SyncVar(hook = nameof(SetNewPhaseById))] private int currentPhaseId = -420;
    public void SetNewPhaseById(int oldVal, int newVal)
    {
        SetNewMatchPhase(MatchPhaseManager.GetNewMatchPhase(currentPhaseId)); 
        Debug.Log("new Phase ID = " + newVal);
    }
    public MatchPhase currentPhase { get; private set; }
    
    [SerializeField] public MatchGuiManager guiManager;
    public float matchTimer
    {
        get => _matchTimer;
        set
        {
            _matchTimer = value;
            Instance.guiManager.SetTimer(_matchTimer);
        }
    }

    public static MatchManager Instance { get; private set; }
    public Player winnerPlayer { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple MatchManager have been created. Destroying the script of " + gameObject.name,
                gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        
        BroadcastNewMatchPhase(null);
    }

    
    private void Update()
    {
        if (isServer)
            UpdateServer();
        
        currentPhase?.UpdateState(Time.deltaTime);
    }

    [Server]
    private void UpdateServer()
    {
        State nextPhase = currentPhase?.GetCurrentState();
        
        if (currentPhase != nextPhase)
            BroadcastNewMatchPhase((MatchPhase) nextPhase);
    }
    
    
    [Server]
    public void BroadcastNewMatchPhase(MatchPhase newPhase)
    {
        currentPhaseId = MatchPhaseManager.GetPhaseId(newPhase);
    }

    [Client]
    private void SetNewMatchPhase(MatchPhase newPhase)
    {
        if (isServer)
            SetAllPlayersAsNotReady();

        currentPhase?.EndState();
        currentPhase = newPhase;
        currentPhase?.StartState();

        guiManager.SetupForCurrentPhase();

        foreach (Client client in GameManager.singleton.clients)
        foreach (Player player in client.PlayersManager.players)
            player.SetupForCurrentPhase();
    }
    
    [Server]
    public void SetAllPlayersAsNotReady()
    {
        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.PlayersManager.players)
                player.CmdSetReady(false);
    }


    /*public bool AreAllPlayersReady()
    {
        foreach (Client client in GameManager.singleton.clients)
        {
            HashSet<Player> players = client.playerManager.players;
            if (players == null || players.Count <= 0) return false;
            if (!players.All(player => player.isReady)) return false;
        }

        return true;
    }*/
    
    /*public void MatchFinished(Player winner)
    {
        winnerPlayer = winner;
        Debug.Log(winner.gameObject.name + "won");
        guiManager.ShowEndScreen(winner.gameObject.name);
    }*/

    /*public void KillActiveCharacters()
    {
        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.playerManager.players)
                player.character.Suicide();
    }*/
}