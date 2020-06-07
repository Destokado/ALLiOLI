using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private float force = 100f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float upwwardsModifier = .1f;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        //TODO: DIsplay VFX
       /* 
        foreach (Collider collider  in Physics.OverlapSphere(transform.position,radius))
        {
            if (collider.GetComponent<Rigidbody>() != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, collider.transform.position - transform.position, out hit,
                    radius))
                {
                    if (hit.collider == collider)
                    {
                        collider.GetComponent<Rigidbody>().AddExplosionForce(force,transform.position,radius,upwwardsModifier,forceMode);
                        //KILL CHARACTER
                    }
                }
            }
            
        }*/
        
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
    }
}
