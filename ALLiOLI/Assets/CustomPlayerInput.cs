using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInput : MonoBehaviour
{
    public static List<CustomPlayerInput> inputsWaitingForPlayers = new List<CustomPlayerInput>();
    public Player player {
        get => _player;
        set {
            if (_player != null && value != null)
                Debug.LogWarning("Trying to assign a player to a CustomPlayerInput with a player already assigned. Operation Cancelled.", this);
            else {
                _player = value;
                
                if (value == null) inputsWaitingForPlayers.Add(this); 
                else inputsWaitingForPlayers.Remove(this);
            }
            SetDynamicName();
        }
    }
    private Player _player;
    private Vector2 cameraMovement;
    public PlayerInput playerInput { get; private set; }

    private void Awake()
    {
        inputsWaitingForPlayers.Add(this);
        
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        SetDynamicName();
    }

    private void SetDynamicName()
    {
        string newName;
        if (player == null)
            newName = "PlayerInput of Unknown Player";
        else 
            newName = "PlayerInput of " + player.name;

        gameObject.name = newName;
    }

    private void OnCameraMove(InputValue value)
    {
        if (player == null) return;
        cameraMovement = value.Get<Vector2>();
    }
    
    public float GetAxisCustom(string axisName)
    {
        Vector2 lookDelta = cameraMovement;
        
        lookDelta.Normalize(); //TODO: needed?
 
        switch (axisName)
        {
            case "Mouse X":
                return lookDelta.x;
            case "Mouse Y":
                return lookDelta.y;
        }
        
        return 0;
    }

    private void OnCharacterMove(InputValue value)
    {
        if (player == null) return;
        
        player.character.movementControllerController.horizontalMovementInput = value.Get<Vector2>();
    }

    private void OnTrap()
    {
        if (player == null) return;
        
        State currentState = MatchManager.Instance.currentPhase;
        
        switch (currentState)
        {
            case Battle battle:
                player.ownedTraps.GetBestTrapToActivate(player)?.Activate();
                break;
            case TrapUp trapUp:
                player.SetUpTrapInFront();
                break;
        }
    }

    private void OnReady()
    {
        if (player == null) return;
        player.isReady = !player.isReady;
    }

    private void OnSuicide()
    {
        if (player == null) return;
        player.character.Suicide();
    }
    
    private void OnJump(InputValue value)
    {
        if (player == null) return;
        player.character.movementControllerController.jumping = value.isPressed;
    }
    

}
