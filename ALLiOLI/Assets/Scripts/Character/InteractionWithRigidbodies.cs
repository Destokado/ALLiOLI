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
        Rigidbody otherRb = hit.collider.attachedRigidbody;
        if (otherRb == null || otherRb.isKinematic)
            return;
        ApplyForce(hit, otherRb);

        //KillZone otherKz = hit.collider.gameObject.GetComponent<KillZone>();
        //if (otherKz)
        //    otherKz.CollidedWith(GetComponent<Character>(), hit);
    }

    private void ApplyForce(ControllerColliderHit hit, Rigidbody otherRb)
    {
        // Calculate direction of the push
        Vector3 pushDir = hit.moveDirection;

        // Vertical push (weight) //TODO: decide if this is needed --> Can cause bugs with physics
        if (hit.moveDirection.y < 0)
            pushDir.y = Mathf.Abs(pushDir.y) * Physics.gravity.y * weight;

        // Apply the push
        otherRb.AddForceAtPosition(pushDir * (pushPower * characterController.velocity.magnitude), hit.point,
            ForceMode.Force);
    }
}
