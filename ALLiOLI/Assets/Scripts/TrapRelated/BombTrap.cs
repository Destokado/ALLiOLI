using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Mirror;
using UnityEngine;

public class BombTrap : SimpleAnimationTrap
{

    private SimpleAnimationsManager animManager;
    [SerializeField] private StudioEventEmitter reloadEmitter;


    [Header("Explosion configuration")] 
    [SerializeField] private Transform bombStartPosition;
    [SerializeField] private float force = 200;
    [SerializeField] private float bombEffectRadius = 3.14f;
    [SerializeField] private float upwardsModifier = .1f;
    //[SerializeField] private ForceMode forceMode = ForceMode.Impulse;
    [SerializeField] private LayerMask ignoredBlockingLayers;

    
    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        if (reloadEmitter.Event != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, reloadEmitter.Event);
        else
        {
            Debug.LogWarning($" The reloadEmitter.Event  is null in {gameObject.name}");
        }
        //TODO: Stop VFX (maybe it needs to be in RPC)
    }
    
    public override void Activate()
    {
        base.Activate();
        
        RpcExplosionOnClients();

    }

    [ClientRpc]
    private void RpcExplosionOnClients()
    {
        //TODO: Display VFX
        
        foreach (var collider in GetCurrentlyEffectedColliders())
        {
            Character character = collider.GetComponentInParent<Character>();
            if (character && character.hasAuthority)
                character.Kill();

            if (collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddExplosionForce(force, transform.position, bombEffectRadius, upwardsModifier,
                    ForceMode.Impulse);
                Debug.DrawRay(collider.transform.position, collider.transform.position - bombStartPosition.position,
                    new Color(1f, 0.5f, 0f), 3f);
            }
        }
    }


    private List<Collider> GetCurrentlyEffectedColliders()
    {
        List<Collider> colliders = new List<Collider>();

        foreach (Collider collider in Physics.OverlapSphere(bombStartPosition.position, bombEffectRadius))
        {
            if (collider.attachedRigidbody != null)
            {
                
                
                if (Physics.Linecast(bombStartPosition.position, collider.transform.position, ignoredBlockingLayers, QueryTriggerInteraction.Ignore))
                {
                    // Noting intersects
                    colliders.Add(collider);
                }
            }
        }

        return colliders;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bombStartPosition.position, bombEffectRadius);
    }
}
