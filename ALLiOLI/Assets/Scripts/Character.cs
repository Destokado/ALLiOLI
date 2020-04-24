using System;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : MonoBehaviour
{
    [SerializeField] public Transform cameraTarget;

    public Player owner;

    //private Rigidbody rb { get; set; }
    public CharacterMovementController movementControllerController { get; private set; }

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        movementControllerController = GetComponent<CharacterMovementController>();
    }

}