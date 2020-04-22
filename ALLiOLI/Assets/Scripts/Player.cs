using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    
    [SerializeField] private Character character;
    [SerializeField] private ATrap exampleTrap;
    [SerializeField] private GameObject characterPrefab;
    private new ThirdPersonCamera camera;
    private PlayerInput playerInput;

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
        //TODO:If there isn't any trap activatable, then activate the nearest one if it isn't on CD
        exampleTrap.Activate();

    }
}
