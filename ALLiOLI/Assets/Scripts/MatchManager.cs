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
    }
    public MatchPhase CurrentPhase { get; private set; }
    private TrapManager allTraps = new TrapManager();
    [SerializeField] public MatchGuiManager guiManager; // General GUI (not the player specific one)


    public float MatchTimer
    {
        get => _matchTimer;
        set
        {
            _matchTimer = value;
            Instance.guiManager.SetTimer(_matchTimer);
        }
    }

    public static MatchManager Instance { get; private set; }
    
    public bool ThereIsWinner => WinnerPlayerNetId != 0u; // '0u' is the default value for 'uint' type
    [field: SyncVar(hook = nameof(UpdatedWinner))] public uint WinnerPlayerNetId { get; private set; }
    private void UpdatedWinner(uint oldId, uint newId) {
        Player winner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newId);
        Debug.Log("Got the winner id: " + WinnerPlayerNetId + ". So, is there a winner? " + ThereIsWinner + ", and it is " + (winner != null? winner.gameObject.name : "NULL"));
        guiManager.UpdateEndScreen();
    }

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
        //allTraps.AddRange(FindObjectsOfType<Trap>().ToList());
        BroadcastNewMatchPhase(null);
    }

    
    private void Update()
    {
        if (isServer)
            UpdateServer();
        
        CurrentPhase?.UpdateState(Time.deltaTime);
    }

    [Server]
    private void UpdateServer()
    {
        State nextPhase = CurrentPhase?.GetCurrentState();
        
        if (CurrentPhase != nextPhase)
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

        CurrentPhase?.EndState();
        
        CurrentPhase = newPhase;
        
        if (CurrentPhase == null)
            return;
        
        CurrentPhase.StartState();
        if (isServer) CurrentPhase.ServerStartState();

        guiManager.SetupForCurrentPhase(); // General GUI

        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.PlayersManager.players)
                player.SetupForCurrentPhase(); // Player's UI
    }
    
    [Server]
    public void SetAllPlayersAsNotReady()
    {
        foreach (Client client in GameManager.singleton.clients)
        foreach (Player player in client.PlayersManager.players)
        {
            //player.CmdSetReady(false);
            player.isReady = false;
        }
    }
    
    [Server]
    public void FlagAtSpawn(Player carrier)
    {
        if (ThereIsWinner)
            return;
        
        WinnerPlayerNetId = carrier.netId;
    }

    /*[Command]
    public void CmdActivateTrap(Trap trap)
    {
        trap.RpcActivate();
    }*/

    /*public void KillActiveCharacters()
    {
        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.playerManager.players)
                player.character.Suicide();
    }*/
}