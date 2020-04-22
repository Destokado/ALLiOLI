using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchManager : MonoBehaviour
{
    private State _currentState;
    public PlayerInputManager playerInputManager { get; private set; }

    public static MatchManager Instance { get; private set; }
    public Player winnerPlayer { get; private set; }
    [SerializeField] public MatchGuiManager guiManager;

    public float countdownTimer { 
        get => _countdownTimer;
        set { _countdownTimer = value; MatchManager.Instance.guiManager.SetTimer(_countdownTimer); } 
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
            _currentState = new WaitingForPlayers();
            playerInputManager = GetComponent<PlayerInputManager>();
        }
    }

    private void Start()
    {
        _currentState.StartState();
    }

    private void Update()
    {
        State nextPhase = _currentState.GetCurrentState();
        if (_currentState != nextPhase)
            SetupMatchPhase(nextPhase);
        _currentState.UpdateState(Time.deltaTime);
    }

    private void SetupMatchPhase(State nextPhase)
    {
        _currentState?.EndState();
        _currentState = nextPhase;
        _currentState?.StartState();
    }
    
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined with input device: " + playerInput.devices[0], playerInput.gameObject);
        //Player player = playerInput.GetComponent<Player>();
        //player.Setup();
    }

    private void OnPlayerJoinedOnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player left with input device: " + playerInput.devices[0], playerInput.gameObject);
        
    }
    
}
