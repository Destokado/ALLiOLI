using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private new ThirdPersonCamera camera;
    private PlayerInput playerInput;
    
	[Space]
    [SerializeField] private PlayerGuiManager playerGui;

    [Space]
    [SerializeField] private GameObject characterPrefab;
    public Character character { get; private set; }
    
    [Space]
    [SerializeField] private float maxDistanceToInteractWithTrap = 10;
    [SerializeField] private LayerMask layersThatCanInterfereWithInteractions;
    private TrapManager ownedTraps = new TrapManager();
    private int maxOwnableTraps => 10 / MatchManager.Instance.players.Count;
    private Trap trapInFront;
    private GameObject lastObjectInFront;
    
    public Color color
    {
        get { return _color;}
        private set
        {
            _color = value;
            playerGui.SetColor(_color);
        }
    }
    private Color _color;
    
    public bool isReady;

    public void Setup(Color color)
    {
        playerInput = GetComponent<PlayerInput>();
        camera = playerInput.camera.gameObject.GetComponent<ThirdPersonCamera>();
        
        this.color = color;
        gameObject.name = "Player " + playerInput.playerIndex + " - " + playerInput.user.controlScheme;
        
        SpawnNewCharacter();
        
        camera.Setup(character.cameraTarget);
    }

    
    private void Update()
    {
        UpdateObjectsInFront();
    }
    
    private void UpdateObjectsInFront()
    {
        Ray ray = new Ray(character.cameraTarget.position, camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceToInteractWithTrap, layersThatCanInterfereWithInteractions)) {
            if (lastObjectInFront != hit.collider.gameObject)
            {
                lastObjectInFront = hit.collider.gameObject;
                trapInFront = hit.transform.GetComponentInParent<Trap>();
            }
        } else {
            lastObjectInFront = null;
            trapInFront = null;
        }
    }

    public void SpawnNewCharacter()
    {
        this.character = Spawner.Instance.Spawn(characterPrefab).GetComponent<Character>();
    }
    
    private void OnCameraMove(InputValue value)
    {
        camera.movement = value.Get<Vector2>();
    }

    private void OnCharacterMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 targetDirection = new Vector3(input.x, 0f, input.y);
        targetDirection = camera.gameObject.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;
        character.movement = targetDirection;
    }

    private void OnTrap()
    {
        State currentState = MatchManager.Instance.currentState;

        switch (currentState)
        {
            case Battle battle:
                ownedTraps.GetBestTrapToActivate()?.Activate();
                break;
            case TrapUp trapUp:
                SetUpTrapInFront();
                break;
        }
    }

    private void SetUpTrapInFront()
    {
        if (trapInFront == null) 
            return;
        
        if (!ownedTraps.Remove(trapInFront))
            ownedTraps.Add(trapInFront);

        playerGui.SetCurrentNumberOfTraps(ownedTraps.Count, maxOwnableTraps);
        DebugPro.LogEnumerable(ownedTraps, ", ", "The current owned traps for the player " + gameObject.name +" are: ", gameObject);
    }

    private void OnReady()
    {
        isReady = !isReady;
    }
}