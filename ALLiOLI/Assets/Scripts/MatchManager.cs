using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MatchManager : MonoBehaviour
{
    private MatchPhase currentMatchPhase;
    public PlayerInputManager playerInputManager { get; private set; }

    public static MatchManager Instance { get; private set; }
    public Player winnerPlayer { get; private set; }

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
            currentMatchPhase = new WaitingForPlayers();
            playerInputManager = GetComponent<PlayerInputManager>();
        }
    }

    private void Start()
    {
        currentMatchPhase.StartPhase();
    }

    private void Update()
    {
        MatchPhase nextPhase = currentMatchPhase.GetCurrentPhase();
        if (currentMatchPhase != nextPhase)
            SetupMatchPhase(nextPhase);
        currentMatchPhase.UpdatePhase(Time.deltaTime);
    }

    private void SetupMatchPhase(MatchPhase nextPhase)
    {
        currentMatchPhase?.EndPhase();
        currentMatchPhase = nextPhase;
        currentMatchPhase?.StartPhase();
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
