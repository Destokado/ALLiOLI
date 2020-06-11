using System;
using FMOD;
using Mirror;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SafeZone : MonoBehaviour
{
    /*
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) return;
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
        character.Kill(Vector3.zero, character.transform.position);
        //Destroy(other.gameObject, 3f);
    }*/

    private void OnTriggerExit(Collider other)
    {
        Flag flag = other.GetComponent<Flag>();
        if (flag) //If the flag falls off the map, reset it
        {
          //  if (hasAuthority)
           // {
                Debug.Log($"The flag of the player {flag.Owner.name} has fallen into the void, resetting it");
                flag.Reset();
                return;
            //}
        }
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) return;
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
        character.Kill(Vector3.zero, character.transform.position);
       // Destroy(character.gameObject,3);
        //NetworkServer.UnSpawn(character.gameObject);
    }
}