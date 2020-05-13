using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class InteractionWithRigidbodies : MonoBehaviour
{
    
    private CharacterController characterController;
    [SerializeField] float pushPower = 3f;
    [SerializeField] float weight = 1f;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Do not push objects without rigidbody
        if (body == null || body.isKinematic) return;

        // Calculate direction of the push
        Vector3 pushDir = hit.moveDirection;
        
        // Vertical push (weight) //TODO: decide if this is needed --> Can cause bugs with physics
        if (hit.moveDirection.y < 0)
            pushDir.y = Mathf.Abs(pushDir.y) * Physics.gravity.y * weight;

        // Apply the push
        body.AddForceAtPosition(pushDir * (pushPower * characterController.velocity.magnitude), hit.point, ForceMode.Force);
        
    }
}
