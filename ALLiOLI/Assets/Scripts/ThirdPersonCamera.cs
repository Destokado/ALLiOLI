using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
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
    [SerializeField] private float distanceToTarget = 10;
    [Space]
    [SerializeField] private LayerMask layersThatCanClipCamera;
    [SerializeField] private float offsetOnCollision = 0.5f;

    [HideInInspector] public Vector2 movement;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Setup(Transform target)
    {
        this.target = target;
    }

    void LateUpdate()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        float yaw = (eulerAngles.y + 180.0f);
        float pitch = eulerAngles.x;

        yaw += yawRotationalSpeed * movement.x * Time.deltaTime;
        yaw *= Mathf.Deg2Rad;
        if (pitch > 180.0f)
            pitch -= 360.0f;
        pitch += pitchRotationalSpeed * (-movement.y) * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        pitch *= Mathf.Deg2Rad;
        Vector3 desiredPosition = target.position + new Vector3(Mathf.Sin(yaw) * Mathf.Cos(pitch) * distanceToTarget,  Mathf.Sin(pitch) * distanceToTarget, Mathf.Cos(yaw) * Mathf.Cos(pitch) * distanceToTarget);
        Vector3 direction = target.position - desiredPosition;

        direction /= distanceToTarget; //TODO: Isn't it 'normalize'?

        Ray ray = new Ray(target.position, -direction);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, distanceToTarget, layersThatCanClipCamera))
        {
            desiredPosition = raycastHit.point + direction * offsetOnCollision;
        }
            
        transform.forward = direction;
        transform.position = desiredPosition;
    }


}
