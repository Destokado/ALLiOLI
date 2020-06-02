using System;
using Mirror;
using UnityEngine;

// IMPORTANT NOTE: The flag class only stores information and does changes to his own information.
// It is not meant to perform any operation or change in any other object

public class Flag : NetworkBehaviour
{
    public bool hasOwner => owner != null;
    public Player owner { get; private set; } //Last player that carried the flag
    public Character carrier //Currently carrying the flag
    {
        get => _carrier;
        private set
        {
            if (value == _carrier) return;
            if (_carrier != null) _carrier.hasFlag = false;
            _carrier = value;
            if (_carrier != null) _carrier.hasFlag = true;
        }
    }
    private Character _carrier;
    
    [SyncVar] public bool canBePicked = true;
   

   

    private void OnTriggerEnter(Collider other)
    {
        if (carrier || !canBePicked || !isServer) return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead)
            return;
        
        FlagManager.Instance.FlagPickedBy(character);
    }

    [Server]
    public void AttachTo(Character character)
    {
        owner = character.Owner;
        carrier = character;
        canBePicked = false;
        Client.LocalClient.SoundManager.PlayOneShotAllClients(SoundEventPaths.pickUpPath,transform.position,null);
    }

    [Server]
    public void Detach()
    {
        carrier = null;
        canBePicked = true;
        
        // The owner only changes one the flag is picked, so the flag can get into the spawn point by physics, ... and the last player that owned it would win
        // owner = null; 
    }

    [Server]
    public void Reset()
    {
        canBePicked = true;
        carrier = null;
        owner = null;
    }
}