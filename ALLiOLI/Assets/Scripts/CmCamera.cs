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
