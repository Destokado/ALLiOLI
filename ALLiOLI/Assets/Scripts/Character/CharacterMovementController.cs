using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Character))]
public class CharacterMovementController : MonoBehaviour
{
    [HideInInspector] public Vector2 horizontalMovementInput;
    public bool jumping { get; set; }

    public CharacterController characterController { get; private set; }
    public Character character { get; private set; }
    
    [Header("Configuration")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;
    [Space]
    [SerializeField] private Animator animator;
    
    private bool onGround = false;
    private float verticalSpeed = 0f;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();
    }

    void Update()
    {
        Vector3 direction = GetDirectionRelativeToTheCamera();
        
        // Calculate walking the distance
        Vector3 displacement = direction * (walkSpeed * Time.deltaTime);

        //Jump
        if(onGround && jumping)
            verticalSpeed=jumpSpeed;
    
        // Apply gravity
        verticalSpeed+=Physics.gravity.y*Time.deltaTime;
        displacement.y=verticalSpeed*Time.deltaTime;
    
        if (Math.Abs(displacement.y) < characterController.minMoveDistance)
            Debug.LogWarning("WAT displacement.y = " + displacement.y);
        
        // Apply Movement to Player
        CollisionFlags collisionFlags=characterController.Move(displacement);

        //Apply rotation to Player
        if (Math.Abs(direction.x) > 0.001f || Math.Abs(direction.y) > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);

        // Process vertical collisions
        if((collisionFlags & CollisionFlags.Below)!=0) {
            onGround=true;
            verticalSpeed=0.0f;
        } else
            onGround=false;
        if((collisionFlags & CollisionFlags.Above)!=0 &&  verticalSpeed>0.0f)
            verticalSpeed=0.0f;

        GiveStateToAnimations(displacement);
    }

    private void GiveStateToAnimations(Vector3 displacement)
    {
        animator.SetFloat("HorMove", Mathf.Abs(displacement.ToVector2WithoutY().magnitude)*10);
        animator.SetBool("Grounded", onGround);
    }

    private Vector3 GetDirectionRelativeToTheCamera()
    {
        Vector3 targetDirection = new Vector3(horizontalMovementInput.x, 0f, horizontalMovementInput.y);
        targetDirection = character.owner.playerCamera.gameObject.transform.TransformDirection(targetDirection);
        /// targetDirection = character.owner.playerCamera.GetComponent<CinemachineFreeLook>().
        targetDirection.y = 0.0f;
        return targetDirection.normalized;
    }
}
