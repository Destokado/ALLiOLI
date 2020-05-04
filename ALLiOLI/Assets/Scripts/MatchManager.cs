using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchManager : MonoBehaviour
{
    public MatchPhase currentPhase { get; private set; }
    public PlayerInputManager playerInputManager { get; private set; }
    public HashSet<Player> players { get; private set; }

    public float matchTimer { get => _matchTimer;
        set { _matchTimer = value; Instance.guiManager.SetTimer(_matchTimer);  } }
    private float _matchTimer;

    public static MatchManager Instance { get; private set; }
    public Player winnerPlayer { get; private set; }
    [SerializeField] public MatchGuiManager guiManager;
    [SerializeField] public Color[] playerColors;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple MatchManager have been created. Destroying the script of " + gameObject.name, gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
            playerInputManager = GetComponent<PlayerInputManager>();
            players = new HashSet<Player>();
        }
    }

    private void Start()
    {
        SetNewMatchPhase(new WaitingForPlayers());
    }

    private void Update()
    {
        State nextPhase = currentPhase.GetCurrentState();
        if (currentPhase != nextPhase)
            SetNewMatchPhase((MatchPhase) nextPhase);
        currentPhase.UpdateState(Time.deltaTime);
    }

    public void SetNewMatchPhase(MatchPhase nextPhase)
    {
        SetAllPlayersAsNotReady();

        currentPhase?.EndState();
        currentPhase = nextPhase;
        currentPhase?.StartState();

        guiManager.SetupForCurrentPhase();

        foreach (Player player in players)
            player.SetupForCurrentPhase();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Player player = playerInput.GetComponent<Player>();
        players.Add(player);
        player.Setup(playerColors[playerInput.playerIndex]);

        SetAllPlayersAsNotReady();

        Debug.Log("Player " + playerInput.playerIndex + " joined with input device: " + playerInput.devices[0],
            playerInput.gameObject);

        Debug.Log("Player " + playerInput.playerIndex + " joined with input device: " + playerInput.devices[0],
            playerInput.gameObject);
    }

    private void OnPlayerJoinedOnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left with input device: " + playerInput.devices[0], playerInput.gameObject);
        players.Remove(playerInput.gameObject.GetComponent<Player>());
    }

    public bool IsAnyPlayerReady()
    {
        return players.Any(player => player.isReady);
    }

    public bool AreAllPlayersReady()
    {
        if (players == null || players.Count <= 0) return false;
        return players.All(player => player.isReady);
    }

    public void SetAllPlayersAsNotReady()
    {
        foreach (Player player in players)
            player.isReady = false;
    }

    public void MatchFinished(Player winner)
    {
        winnerPlayer = winner;
        Debug.Log(winner.gameObject.name + "won");
        guiManager.ShowEndScreen(winner.gameObject.name);
    }

    public void KillPlayers()
    {
        foreach (Player p in players)
        {
            p.character.Die();
        }
    }
}