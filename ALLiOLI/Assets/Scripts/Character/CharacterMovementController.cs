using System;
using FMOD;
using FMOD.Studio;
using Mirror;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Character))]

public class CharacterMovementController : NetworkBehaviour
{
    [Space] [SerializeField] private Animator animator;
    
    [NonSerialized] public Vector2 horizontalMovementInput;
    [SerializeField] private float jumpSpeed;

    private bool onGround;
    private float verticalSpeed;

    [Header("Configuration")] [SerializeField]
    private float walkSpeed;

    public bool jumping { get; set; }

    private EventInstance runningEvent;
    private bool running
    {
        get => running;
        set
        {
            if (!runningEvent.isValid())
            {
                runningEvent =
                    Client.LocalClient.SoundManager.PlayEventMovingAllClients(SoundEventPaths.runPath, transform);
            }
            if (value)
            {
                PLAYBACK_STATE state;
                runningEvent.getPlaybackState(out state);
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    runningEvent.start();
                }
            }
            else
            {
                runningEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            }
        }
    }

    public CharacterController CharacterController { get; private set; }
    public Character Character { get; private set; }
    [Header("Rotation")]
    [SerializeField] private float turnSmoothTime = .1f;
    [SerializeField] private float turnSmoothVelocity;

  
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

        float fallingDistance=0f; 

        //Jump
        if (onGround && jumping)
        {
            verticalSpeed = jumpSpeed;
            
            Client.LocalClient.SoundManager.PlayOneShotMovingAllClients(SoundEventPaths.jumpPath,this.transform);
            fallingDistance= transform.position.y;

        }

        // Apply gravity
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        displacement.y = verticalSpeed * Time.deltaTime;

        
        Vector3 horDisplacement = displacement.WithY(0);
        bool walking = onGround && horDisplacement.magnitude > CharacterController.minMoveDistance;
        running = walking;
        
        // Apply Movement to Player
        CollisionFlags collisionFlags = CharacterController.Move(displacement);

        //Apply rotation to Player
        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);
        }

        // Process vertical collisions
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            SoundManagerParameter[] parameters = new SoundManagerParameter[1];
            //Calculates de distance of the fall
            fallingDistance = fallingDistance + transform.position.y;
            //The Max in the Clamp must be the Max range of the Event in FMOD.
            fallingDistance=Mathf.Clamp(fallingDistance, 0, 2);
            parameters[0]= new SoundManagerParameter("Height", fallingDistance);
            if(onGround==false) Client.LocalClient.SoundManager.PlayOneShotAllClients(SoundEventPaths.landPath,transform.position,parameters);
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
        Vector3 targetDirection = new Vector3(horizontalMovementInput.x, 0f, horizontalMovementInput.y);
        targetDirection = Character.Owner.HumanLocalPlayer.Camera.gameObject.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;
        return targetDirection.normalized;
    }
}