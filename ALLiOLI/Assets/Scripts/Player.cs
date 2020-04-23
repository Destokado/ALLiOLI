using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public Character character { get; private set; }
    [SerializeField] private GameObject characterPrefab;
    private new ThirdPersonCamera camera;
    private PlayerInput playerInput;
    [SerializeField] private List<ATrap> ownedTraps;

    private void OnCameraMove(InputValue value)
    {
        camera.movement = value.Get<Vector2>();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        camera = playerInput.camera.gameObject.GetComponent<ThirdPersonCamera>();

        SpawnNewCharacter();
        camera.Setup(character);
    }

    public void SpawnNewCharacter()
    {
        this.character = Spawner.Instance.Spawn(characterPrefab).GetComponent<Character>();
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
        Debug.Log("Trap button pressed");
        State state = MatchManager.Instance.currentState;

        if (state is Battle)
        {
            character.ActivateTrap(ownedTraps);
        }
        else if (state is TrapUp)
        {
            ownedTraps = character.SetUpTrap(camera.transform.position, camera.transform.forward, ownedTraps);
        }
    }
}