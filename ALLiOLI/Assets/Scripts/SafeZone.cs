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
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) return;
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
        character.Kill(/*Vector3.zero, character.transform.position*/);
       // Destroy(character.gameObject,3);
        //NetworkServer.UnSpawn(character.gameObject);
    }
}