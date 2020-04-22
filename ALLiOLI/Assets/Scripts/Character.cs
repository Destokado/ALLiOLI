using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public Vector3 movement { get { return _movement;} set { _movement = value.normalized; } }
    private Vector3 _movement;
    [SerializeField] private float speed = 10;
    [SerializeField] private List<ATrap> ownedTraps;
    private Rigidbody rb { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    public void ActivateTrap()
    {
        //TODO:If there isn't any trap activatable, then activate the nearest one if it isn't on CD

        throw new NotImplementedException();
    }

    public void SetUpTrap()
    {
        ATrap trap = TrapManager.Instance.GetClosestTrap(this.transform);
        if (ownedTraps.Remove(trap))
        {
            trap.placed = false;
        }
        else
        {
            trap.placed = true;
            ownedTraps.Add(trap);
        }
    }
}
