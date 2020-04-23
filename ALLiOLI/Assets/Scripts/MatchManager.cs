using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchManager : MonoBehaviour
{
    public State currentState { get; private set; }
    public PlayerInputManager playerInputManager { get; private set; }
    public List<Player> players { get; private set; }
    public static MatchManager Instance { get; private set; }
    public Player winnerPlayer { get; private set; }
    [SerializeField] public MatchGuiManager guiManager;

    public float countdownTimer { 
        get => _countdownTimer;
        set { _countdownTimer = value; Instance.guiManager.SetTimer(_countdownTimer); } 
    }
    private float _countdownTimer;
    
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
            currentState = new WaitingForPlayers();
            playerInputManager = GetComponent<PlayerInputManager>();
            players= new List<Player>();
        }
    }

    private void Start()
    {
        currentState.StartState();
    }

    private void Update()
    {
        State nextPhase = currentState.GetCurrentState();
        if (currentState != nextPhase)
            SetupMatchPhase(nextPhase);
        currentState.UpdateState(Time.deltaTime);
    }

    private void SetupMatchPhase(State nextPhase)
    {
        currentState?.EndState();
        currentState = nextPhase;
        currentState?.StartState();
    }
    
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined with input device: " + playerInput.devices[0], playerInput.gameObject);
        players.Add(playerInput.GetComponent<Player>());
        //Player player = playerInput.GetComponent<Player>();
        //player.Setup();
    }

    private void OnPlayerJoinedOnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left with input device: " + playerInput.devices[0], playerInput.gameObject);
        players.Remove(playerInput.gameObject.GetComponent<Player>());

    }
    
}
