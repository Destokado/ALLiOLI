﻿using System;
 using System.Collections;
 using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : MonoBehaviour
{
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public Transform flagPosition;
    
    [HideInInspector] public Flag flag;
    [HideInInspector] public Player owner;
    
    public bool isDead { get; private set; }

    //private Rigidbody rb { get; set; }
    public CharacterMovementController movementControllerController { get; private set; }

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        movementControllerController = GetComponent<CharacterMovementController>();
    }

    public void Die(Vector3 impact, Vector3 impactPoint)
    {
        if (!isDead)
            StartCoroutine(DieCoroutine(impact, impactPoint));
    }
    
    private IEnumerator DieCoroutine(Vector3 impact, Vector3 impactPoint)
    {
        isDead = true;
        if (flag != null) flag.Detach();
        movementControllerController.enabled = false;

        movementControllerController.enabled = false;
        movementControllerController.characterController.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForceAtPosition(impact, impactPoint, ForceMode.Impulse);
        
        yield return new WaitForSeconds(1.5f);
        
        owner.SpawnNewCharacter();
    }

    public void Suicide()
    {
        Die(Vector3.up*2, transform.position);
    }
}