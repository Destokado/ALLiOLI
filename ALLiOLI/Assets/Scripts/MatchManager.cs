using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : NetworkBehaviour
{
    private float _matchTimer;
    [SyncVar] public MatchPhase currentPhase; // { get; private set; }
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

    private void Start()
    {
        //SetNewMatchPhase(new WaitingForPlayers());
        SetNewMatchPhase(null);
    }

    private void Update()
    {
        State nextPhase = currentPhase?.GetCurrentState();
        if (currentPhase != nextPhase)
            SetNewMatchPhase((MatchPhase) nextPhase);
        currentPhase?.UpdateState(Time.deltaTime);
    }

    public void SetAllPlayersAsNotReady()
    {
        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.playerManager.players)
                player.isReady = false;
    }
    
    public void SetNewMatchPhase(MatchPhase nextPhase)
    {
        SetAllPlayersAsNotReady();

        currentPhase?.EndState();
        currentPhase = nextPhase;
        currentPhase?.StartState();

        guiManager.SetupForCurrentPhase();

        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.playerManager.players)
                player.SetupForCurrentPhase();
    }


    public bool AreAllPlayersReady()
    {
        foreach (Client client in GameManager.singleton.clients)
        {
            HashSet<Player> players = client.playerManager.players;
            if (players == null || players.Count <= 0) return false;
            if (!players.All(player => player.isReady)) return false;
        }

        return true;
    }
    
    public void MatchFinished(Player winner)
    {
        winnerPlayer = winner;
        Debug.Log(winner.gameObject.name + "won");
        guiManager.ShowEndScreen(winner.gameObject.name);
    }

    public void KillActiveCharacters()
    {
        foreach (Client client in GameManager.singleton.clients)
            foreach (Player player in client.playerManager.players)
                player.character.Suicide();
    }
}