using System;

using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public Vector3 movement
    {
        get { return _movement; }
        set { _movement = value.normalized; }
    }

    private Vector3 _movement;
    [SerializeField] private float speed = 10;
    
    [SerializeField] private LayerMask trapLayerMask;
    private Rigidbody rb { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    public void ActivateTrap(List<ATrap>ownedTraps)
    {
        List<ATrap> trapsToActivate = new List<ATrap>();
        //TODO:If there isn't any trap activatable, then activate the nearest one if it isn't on CD

        //Find activatable traps
        foreach (ATrap trap in ownedTraps)
        {
            if (trap.IsActivatable())
            {
                trapsToActivate.Add(trap);
            }
        }

        //Activate activatable traps
        if (trapsToActivate.Count != 0)
        {
            foreach (ATrap trap in trapsToActivate)
            {
                trap.Activate();
            }

            return;
        }

        //There are no activatable traps
        //TODO: Activate the nearest Not-on-CD trap
        GetClosestTrap(ownedTraps,false).Activate();
        throw new NotImplementedException();
    }

    public List<ATrap> SetUpTrap(Vector3 cameraPos, Vector3 cameraDirection,List<ATrap>ownedTraps)
    {
        RaycastHit hit;
        Ray ray = new Ray(cameraPos, cameraDirection);
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 0.1f );
        if (Physics.Raycast(ray, out hit, trapLayerMask))
        {
            Debug.Log(hit.collider.gameObject);
            ATrap trap = hit.transform.GetComponent<ATrap>();
            trap.SetUp();
            if (!ownedTraps.Remove(trap))
            {
                ownedTraps.Add(trap);
            }
        }

        return ownedTraps;
    }

     private ATrap GetClosestTrap(List<ATrap>ownedTraps)
       {
           ATrap closestTrap = ownedTraps[1];
           foreach (ATrap trap in ownedTraps)
           {
               if (Vector3.Distance(trap.transform.position, transform.position) <
                   Vector3.Distance(closestTrap.transform.position, transform.position))
                   closestTrap = trap;
           }
   
           return closestTrap;
       }

    private ATrap GetClosestTrap(List<ATrap>ownedTraps,bool cdConstraint)
    {
        ATrap closestTrap = ownedTraps[1];
        foreach (ATrap trap in ownedTraps)
        {
            if (Vector3.Distance(trap.transform.position, transform.position) <
                Vector3.Distance(closestTrap.transform.position, transform.position) && trap.OnCd == cdConstraint)
                closestTrap = trap;
        }

        if(closestTrap.OnCd!= cdConstraint) throw  new Exception("All traps are on CD!");
        return closestTrap;
    }
}