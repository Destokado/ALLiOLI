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

    public void Die()
    {
        if (!isDead)
            StartCoroutine(DieCoroutine());
    }
    
    private IEnumerator DieCoroutine()
    {
        isDead = true;
        if (flag != null) flag.Detach();
        movementControllerController.enabled = false;

        movementControllerController.enabled = false;
        movementControllerController.characterController.enabled = false;
        gameObject.AddComponent<Rigidbody>();
        
        yield return new WaitForSeconds(1.5f);
        
        owner.SpawnNewCharacter();
    }

}