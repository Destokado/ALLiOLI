using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CmCamera : MonoBehaviour
{
    
    private PlayerInput playerInput;
    private Vector2 LookDelta;
    [SerializeField] private CinemachineFreeLook freeLook;
    public Vector2 cameraMovement;

    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float GetAxisCustom(string axisName)
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


    public void SetTarget(Transform target, Transform follow)
    {
        freeLook.Follow = follow;
        freeLook.LookAt = target;
    }

    public void SetLayer(int layer, Camera camera)
    {
        
        camera.gameObject.SetLayerRecursively(layer); 
        camera.cullingMask |= 1 << layer;
    }
}
