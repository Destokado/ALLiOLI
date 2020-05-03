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

    private void OnTriggerEnter(Collider other)
    {
        if (rigidbody)
            if (rigidbody.velocity.magnitude < minimumVelocityToKill)
                return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;
        
        character.Die();

        string additionalReport = rigidbody? "\nVelocity = " + rigidbody.velocity.magnitude + " (required " + minimumVelocityToKill + " to kill)" : "";
        Debug.Log(character.name + " was killed by" + transform.parent.gameObject.name + additionalReport, this);
    }

}
