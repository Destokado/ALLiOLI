using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Character))]
public class CharacterMovementController : NetworkBehaviour
{
    [Space] [SerializeField] private Animator animator;

    [HideInInspector] public Vector2 horizontalMovementInput;
    [SerializeField] private float jumpSpeed;

    private bool onGround;
    private float verticalSpeed;

    [Header("Configuration")] [SerializeField]
    private float walkSpeed;

    public bool jumping { get; set; }

    public CharacterController CharacterController { get; private set; }
    public Character Character { get; private set; }

    private void Awake()
    {
        CharacterController = gameObject.GetComponentRequired<CharacterController>();
        Character = gameObject.GetComponentRequired<Character>();
    }

    private void Update()
    {
        if (hasAuthority)
            AuthorityUpdate();
    }

    private void AuthorityUpdate()
    {
        Vector3 direction = GetDirectionRelativeToTheCamera();

        // Calculate walking the distance
        Vector3 displacement = direction * (walkSpeed * Time.deltaTime);

        //Jump
        if (onGround && jumping)
            verticalSpeed = jumpSpeed;

        // Apply gravity
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        displacement.y = verticalSpeed * Time.deltaTime;

        if (Math.Abs(displacement.y) < CharacterController.minMoveDistance)
            Debug.LogWarning("WAT displacement.y = " + displacement.y);

        // Apply Movement to Player
        CollisionFlags collisionFlags = CharacterController.Move(displacement);

        //Apply rotation to Player
        if (Math.Abs(direction.x) > 0.001f || Math.Abs(direction.y) > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);

        // Process vertical collisions
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            onGround = true;
            verticalSpeed = 0.0f;
        }
        else
        {
            onGround = false;
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0.0f)
            verticalSpeed = 0.0f;

        GiveStateToAnimations(displacement);
    }

    private void GiveStateToAnimations(Vector3 displacement)
    {
        animator.SetFloat("HorMove", Mathf.Abs(displacement.ToVector2WithoutY().magnitude) * 10);
        animator.SetBool("Grounded", onGround);
    }

    private Vector3 GetDirectionRelativeToTheCamera()
    {
        //TODO: Remove if no errors are detected in a lon-term testing
        if (Character == null)
            Debug.LogWarning("Character is null for " + gameObject.name, gameObject);
        if (Character.Owner == null)
            Debug.LogWarning("Character.Owner is null for " + gameObject.name, gameObject);
        if (Character.Owner.HumanLocalPlayer == null)
            Debug.LogWarning("Character.Owner.HumanLocalPlayer is null for " + gameObject.name, gameObject);
        if (Character.Owner.HumanLocalPlayer.Camera == null)
            Debug.LogWarning("Character.Owner.HumanLocalPlayer.Camera is null is null for " + gameObject.name, gameObject);
        
        Vector3 targetDirection = new Vector3(horizontalMovementInput.x, 0f, horizontalMovementInput.y);
        targetDirection = Character.Owner.HumanLocalPlayer.Camera.gameObject.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;
        return targetDirection.normalized;
    }
}