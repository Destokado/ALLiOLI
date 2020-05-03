using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class InteractionWithRigidbodies : MonoBehaviour
{
    
    private CharacterController characterController;
    [SerializeField] float pushPower = 0.5f;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.AddForce(pushDir * (pushPower * characterController.velocity.magnitude), ForceMode.Impulse);
        
    }
}
