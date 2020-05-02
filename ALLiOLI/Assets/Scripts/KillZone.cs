using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Tooltip("Not mandatory")]
    [SerializeField] public new Rigidbody rigidbody;
    [Tooltip("Not mandatory")]
    [SerializeField] private float minimumVelocityToKill = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (rigidbody != null)
            if (rigidbody.velocity.magnitude < minimumVelocityToKill)
                return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;
        
        character.Die();
        Debug.Log(character.name + " was killed by" + transform.parent.gameObject.name);
    }

}
