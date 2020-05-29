using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Tooltip("Not mandatory")]
    [SerializeField] public new Rigidbody rigidbody;
    [Tooltip("Not mandatory")]
    [SerializeField] private float minimumVelocityToKill = 0.75f;
    
    // Should only be called on the server
    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("COLLISION with " + gameObject.transform.parent.name + "/" + gameObject.name, gameObject);

        Character character = other.collider.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;

        if (rigidbody)
        {
            if (rigidbody.velocity.magnitude < minimumVelocityToKill)
            {
                if (!character || character.isDead)  return;
                Debug.Log(character.name + " avoided being killed by" + transform.gameObject.name + "\nVelocity = " + rigidbody.velocity.magnitude + " (required " + minimumVelocityToKill + " to kill)", gameObject);
                return;
            }
        }

        character.ServerDie(other.impulse, other.GetContact(0).point);

        // Print log of the kill
        string additionalReport = rigidbody? "\nVelocity = " + rigidbody.velocity.magnitude + " (required " + minimumVelocityToKill + " to kill)" : "";
        Debug.Log(character.name + " was killed by" + transform.gameObject.name + additionalReport, gameObject);
    }
}
