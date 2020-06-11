using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrap : Trap
{
    private SimpleAnimationsManager animManager;

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
        
        //TODO: Stop VFX
    }

    public override void Activate()
    {
        base.Activate();
        
        RpcExplosionOnClients();

        //TODO: Display VFX

    }

    private void RpcExplosionOnClients()
    {
        foreach (var collider in GetCurrentlyEffectedColliders())
        {
            Character character = collider.GetComponentInParent<Character>();
            if (character && character.hasAuthority)
                character.Kill();
            
            collider.attachedRigidbody.AddExplosionForce(force, transform.position, bombEffectRadius, upwardsModifier,
                ForceMode.Impulse);
            Debug.DrawRay(collider.transform.position, collider.transform.position - bombStartPosition.position,
                new Color(1f, 0.5f, 0f), 3f);
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
