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
  

    public void Setup(Transform target, Transform follow)
    {
        freeLook.Follow = follow;
        freeLook.LookAt = target;
    }
    
}
