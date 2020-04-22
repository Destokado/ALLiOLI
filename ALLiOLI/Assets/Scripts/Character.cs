using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public Vector3 movement { get { return _movement;} set { _movement = value.normalized; } }
    private Vector3 _movement;
    [SerializeField] private float speed = 10;
    
    private Rigidbody rb { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }
}
