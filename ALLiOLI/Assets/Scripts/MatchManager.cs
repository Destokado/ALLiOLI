using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : NetworkBehaviour
{
    private float _matchTimer;
    
    public List<Client> Clients { get; private set; }
    [SerializeField] private Color[] playerColors;

    [SyncVar(hook = nameof(SetNewPhaseById))]
    private int currentPhaseId = MatchPhaseManager.GetPhaseId(new WaitingForPlayers()); // -420; // Dummy value
    public void SetNewPhaseById(int oldVal, int newVal)
    {
        SetNewMatchPhase(MatchPhaseManager.GetNewMatchPhase(currentPhaseId));
    }
    public MatchPhase CurrentPhase { get; private set ; }
    
    public bool IsMatchRunning => (Instance.CurrentPhase != null && Instance.CurrentPhase.Id() >= 0);
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
            Clients = new List<Client>();
        }
    }
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        
        //BroadcastNewMatchPhase(new WaitingForPlayers());
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
    public void FinishAndRestartCurrentPhase()
    {
        MatchPhase phase = CurrentPhase;
        
        if (phase == null)
        {
            phase = MatchPhaseManager.GetNewMatchPhase(currentPhaseId);
            Debug.Log( $"Restarting the CurrentPhase obtained from the currentPhaseId '{currentPhaseId}' ({(phase!=null?phase.GetType().Name:"null")}) as a MatchPhase object.");
        } 
        else
        {
            Debug.Log($"Restarting the CurrentPhase. ({phase.GetType().Name})");
        }

        SetNewMatchPhase(phase);
    }
    
    [Client]
    private void SetNewMatchPhase(MatchPhase newPhase)
    {
        if (newPhase != null)
            Debug.Log($"Setting new phase {newPhase.GetType().Name}");
        else
            Debug.Log("Setting NULL phase");
        
        if (isServer)
            SetAllPlayersAsNotReady();

        CurrentPhase?.EndState();
        
        CurrentPhase = newPhase;
        
        if (CurrentPhase == null)
            return;
        
        CurrentPhase.StartState();
        if (isServer) CurrentPhase.ServerStartState();

        guiManager.SetupForCurrentPhase(); // General GUI

        foreach (Client client in Instance.Clients)
            if (client.PlayersManager != null)
                foreach (Player player in client.PlayersManager.players)
                    player.SetupForCurrentPhase(); // Player's UI
    }
    
    [Server]
    public void SetAllPlayersAsNotReady()
    {
        foreach (Client client in Instance.Clients)
        {
            if (client.PlayersManager != null)
                foreach (Player player in client.PlayersManager.players)
                {
                    //player.CmdSetReady(false);
                    player.isReady = false;
                }
        }
    }
    
    [Server]
    public void FlagAtSpawn(Player carrier)
    {
        if (ThereIsWinner)
            return;
        
        WinnerPlayerNetId = carrier.netId;
    }
    
    public bool AreAllPlayersReady()
    {
        foreach (Client client in Clients)
        foreach (Player player in client.PlayersManager.players)
            if (!player.isReady)
                return false;

        return true;
    }
    
    
    public static int TotalCurrentPlayers => Instance.Clients.Sum(client => client.PlayersManager.players.Count);
    //public static int indexOfLastPlayer = -1;
    
    public Color GetColor(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerColors.Length)
            return playerColors[playerIndex];
        
        //EasyRandom rnd = new EasyRandom(100+playerIndex*42);
        //const float hueMin = 0f;
        //const float hueMax = 1f;

        int steps = 6;
        float hue = 1f / (steps+1f/(steps+1)) * (playerIndex-1);
        while (hue > 1)
            hue -= 1;
        Debug.Log($"HUE {hue}");
        
        const float saturation = 0.85f;
        const float valueBrightness = 1f;

        Color rgb = Color.HSVToRGB(hue, saturation, valueBrightness, true);
        rgb.a = 1f;

        //Random.ColorHSV(0, 1f, 0.7f, 0.9f, 1f, 1f);
        return rgb;
    }

    public void StartMatch()
    {
        //MatchManager.instance.BroadcastNewMatchPhase(new WaitingForPlayers());
    }


}