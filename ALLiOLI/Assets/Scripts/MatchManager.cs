using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchManager : NetworkBehaviour
{
    private float _matchTimer;
    [SyncVar] public MatchPhase currentPhase; // { get; private set; }
    [SerializeField] public MatchGuiManager guiManager;
    public PlayerInputManager playerInputManager { get; private set; }

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
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.enabled = false;
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

    public void SetNewMatchPhase(MatchPhase nextPhase)
    {
        SetAllPlayersAsNotReady();

        currentPhase?.EndState();
        currentPhase = nextPhase;
        currentPhase?.StartState();

        guiManager.SetupForCurrentPhase();

        foreach (Player player in Client.localClient.players)
            player.SetupForCurrentPhase();
    }


    public bool AreAllPlayersReady()
    {
        if (players == null || players.Count <= 0) return false;
        return players.All(player => player.isReady);
    }


    public void MatchFinished(Player winner)
    {
        winnerPlayer = winner;
        Debug.Log(winner.gameObject.name + "won");
        guiManager.ShowEndScreen(winner.gameObject.name);
    }

    public void KillPlayers()
    {
        foreach (Player p in players) p.character.Suicide();
    }
}