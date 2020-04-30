using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger " + other.gameObject.name);
        
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;
        
        character.Die();
        Debug.Log(character.name+" was killed by"+transform.parent.gameObject.name);
    }
}
