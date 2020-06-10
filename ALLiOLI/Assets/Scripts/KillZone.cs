using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] public Trap trap;
    
    // Should only be called on the server
    private void OnCollisionEnter(Collision other)
    {
//        Debug.Log($"OnCollisionEnter of the KillZone named '{transform.gameObject.name}' of the trap '{trap.gameObject.name}' with the object '{other.gameObject.name}.'", gameObject);
        
        Character character = other.collider.GetComponentInParent<Character>();
        
        if (!character || character.isDead || !trap.isActive) 
            return;
        
       
        character.ServerDie(other.impulse, other.GetContact(0).point);
        Debug.Log($"Killed '{character.name}' by the KillZone named '{transform.gameObject.name}' in the trap '{trap.gameObject.name}'.", gameObject);
    }
}
