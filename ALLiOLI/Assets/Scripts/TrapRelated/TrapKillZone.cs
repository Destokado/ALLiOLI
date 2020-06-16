using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TrapKillZone : MonoBehaviour
{
    [SerializeField] public Trap trap;
    [SerializeField] private StudioEventEmitter hitEmitter;

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log($"OnCollisionEnter of the KillZone named '{transform.gameObject.name}' of the trap '{(trap? trap.gameObject.name : "NULL")}' with the object '{other.gameObject.name}.'", gameObject);
        
        Character character = other.collider.GetComponentInParent<Character>();
        
        bool shouldKill = character && !character.isDead && character.Owner.Client.isLocalClient && trap && trap.isActive;

        if (!shouldKill)
        {
            // Debug.Log($"Avoided kill. was the character found? {character != null}. Was the character alive { (character? (!character.isDead).ToString() : "-") }? Did the trap exist? {trap != null}. Was the trap active? { (trap? trap.isActive.ToString() : "-") }");
            return;
        }
        if (hitEmitter != null)
        {
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(GetComponentInParent<Trap>().netId, hitEmitter.Event);

        }
        character.Kill(/*other.impulse, other.GetContact(0).point*/);
        // Debug.Log($"Killed '{character.name}' by the KillZone named '{transform.gameObject.name}' in the trap '{trap.gameObject.name}'.", gameObject);
    }
}
