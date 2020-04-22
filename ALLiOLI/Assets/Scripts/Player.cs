using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    private Character character;
    
    private ThirdPersonCamera thirdPersonCamera;
    private PlayerInput playerInput;

    private void OnCameraMove(InputValue value)
    {
        thirdPersonCamera.cameraMovement = value.Get<Vector2>();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        thirdPersonCamera = playerInput.camera.gameObject.GetComponent<ThirdPersonCamera>();
        
        SpawnNewCharacter();
        thirdPersonCamera.Setup(character);
    }

    public void SpawnNewCharacter()
    {
        this.character = Spawner.Instance.Spawn(characterPrefab).GetComponent<Character>();
    }
}
