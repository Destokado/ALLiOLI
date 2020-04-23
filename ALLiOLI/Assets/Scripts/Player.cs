using System;
using System.Collections;
using System.Collections.Generic;
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
    private Trap trapInFront;
    private GameObject lastObjectInFront;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        camera = playerInput.camera.gameObject.GetComponent<ThirdPersonCamera>();

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
        State state = MatchManager.Instance.currentState;

        switch (state)
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
        
        DebugPro.LogEnumerable(ownedTraps, ", ", "The current owned traps for the player " + gameObject.name +" are: ", gameObject);
    }
}