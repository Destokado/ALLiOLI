using System;
using FMOD;
using Mirror;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Flag flag = other.GetComponent<Flag>();
        if (flag && flag.hasAuthority) //If the flag falls off the map, reset it
        {
            Debug.Log($"The flag of the player {flag.Owner.name} has fallen into the void, resetting it");
            flag.Reset();
            return;
        }

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) 
            return;
        character.Kill( /*Vector3.zero, character.transform.position*/);
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
    }
    
}