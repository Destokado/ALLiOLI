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
            Debug.DrawRay(other.transform.position,Vector3.forward,Color.red,5f);
            flag.Reset();
            return;
        }

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) return;
        if (character.HasFlag)
            character.Owner.Flag
                .Reset(); //If the player falls off with the flag, reset the flag first then kill the player
        character.Kill( /*Vector3.zero, character.transform.position*/);

        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);


        // Destroy(character.gameObject,3);
        //NetworkServer.UnSpawn(character.gameObject);
    }
}