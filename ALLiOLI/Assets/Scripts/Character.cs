using System;

using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] public Transform cameraTarget;
    
    public Vector3 movement
    {
        get { return _movement; }
        set { _movement = value.normalized; }
    }
    private Vector3 _movement;
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