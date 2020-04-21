using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Character character;
    [SerializeField] private ATrap exampleTrap;
    private void OnCameraMove(InputValue value)
    {
    if(cameraManager!=null)    cameraManager.cameraMovement = value.Get<Vector2>();
    }
    
    private void OnTrap()
    {
        Debug.Log("Trap button pressed");
        //TODO:If there isn't any trap activatable, then activate the nearest one if it isn't on CD
        exampleTrap.Activate();
    }
}
