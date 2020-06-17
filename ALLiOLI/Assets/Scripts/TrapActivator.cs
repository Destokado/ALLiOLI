using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mirror;
using UnityEngine;

[SelectionBase]
public class TrapActivator : NetworkBehaviour
{
    [Tooltip("Objects that will be activated and deactivated when needed. Visuals or anything needed.")]
    [SerializeField] private GameObject[] objectsToSynchronize;
    
    [SyncVar (hook = nameof(NewActivationState))] private bool activated = true;
    private void NewActivationState(bool oldValue, bool newValue)
    {
//        Debug.Log("NewActivationState");
        foreach (GameObject obj in objectsToSynchronize)
        {
//            Debug.Log($"Setting {obj} in NewActivationState as {newValue}");
            obj.SetActive(newValue);
        }
    }

    /*private void Start()
    {
        if (isServer)
            activated = true;
    }*/

    private float remainingCoolDown = 0f;
    [SerializeField] private float coolDownTime = 5f;

    private void Update()
    {
        if ( !isServer || (remainingCoolDown <= 0) ) return;

        remainingCoolDown -= Time.deltaTime;
        if (remainingCoolDown <= 0)
            activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated || !isServer || remainingCoolDown > 0) return;

        Character character = other.GetComponentInParent<Character>();
        if (character)
        {
            Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.EventPaths.PickActivator,transform.position,null);
            activated = false;
            remainingCoolDown = coolDownTime;
            character.Owner.trapActivators += 1;
        }
    }
}
