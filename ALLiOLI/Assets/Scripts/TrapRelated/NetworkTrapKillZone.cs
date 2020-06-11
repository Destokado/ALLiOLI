using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkTrapKillZone : NetworkBehaviour
{
    private Trap trap;
    
    [NonSerialized] 
    [SyncVar (hook = nameof(NewTrapNetId))]public uint trapNetId;
    private void NewTrapNetId(uint oldValue, uint newValue)
    {
        AllIOliNetworkManager nwtManager = ((AllIOliNetworkManager) NetworkManager.singleton);
        GameObject trapGameObject = nwtManager.GetGameObject(newValue);
        trap = trapGameObject.GetComponentRequired<Trap>();
    }

    // Should only be called on the server
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"OnCollisionEnter of the KillZone named '{transform.gameObject.name}' of the trap '{(trap? trap.gameObject.name : "NULL")}' with the object '{other.gameObject.name}.'", gameObject);
        
        Character character = other.collider.GetComponentInParent<Character>();
        bool shouldKill = character && !character.isDead && character.Owner.Client.isLocalClient && trap && trap.isActive;

        if (!shouldKill)
        {
            //Debug.Log($"Avoided kill. was the character found? {character != null}. Was the character alive { (character? (!character.isDead).ToString() : "-") }? Did the trap exist? {trap != null}. Was the trap active? { (trap? trap.isActive.ToString() : "-") }");
            return;
        }

        character.Kill(/*other.impulse, other.GetContact(0).point*/);
        //Debug.Log($"Killed '{character.name}' by the KillZone named '{transform.gameObject.name}' in the trap '{trap.gameObject.name}'.", gameObject);
    }
}
