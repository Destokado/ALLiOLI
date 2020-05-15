using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CmCamera : MonoBehaviour
{
    [Space]
    [SerializeField] private float yawRotationalSpeed = 200;
    [SerializeField] private float pitchRotationalSpeed = 200;
    [Space]
    [SerializeField] private float minPitch = 5;
    [SerializeField] private float maxPitch = 75;
    [Space]
    [Tooltip("Not necessary to set if the setup is set trough code")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform follow;
    [SerializeField] private float distanceToTarget = 10;
    [Space]
    [SerializeField] private LayerMask layersThatCanClipCamera;
    [SerializeField] private float offsetOnCollision = 0.5f;

    private PlayerInput playerInput;
    private Vector2 LookDelta;
    [SerializeField] private CinemachineFreeLook freeLook;
    // Start is called before the first frame update
  

    // Update is called once per frame
    

    public void Setup(Transform target, Transform follow)
    {
        freeLook.Follow = follow;
        freeLook.LookAt = target;
       
    }
   
    
}
