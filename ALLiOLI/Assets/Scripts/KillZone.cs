using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private bool canKill = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canKill) return;
        
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;
        
        character.Die();
        Debug.Log(character.name + " was killed by" + transform.parent.gameObject.name);
    }

    public void SetState(bool canKill)
    {
        this.canKill = canKill;
    }
}
