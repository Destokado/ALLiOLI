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

    public bool onGround
    {
        get => _onGround;
        private set
        {
            if (value != _onGround)
            {
                if (value == false)
                    groundLosed = true;
                _onGround = value;
            }
        }
    }
    // ReSharper disable once InconsistentNaming
    private bool _onGround;
    public bool groundLosed;
    
    private float verticalSpeed;

    [Header("Configuration")] [SerializeField]
    private float walkSpeed;

    public bool jumping { get; set; }

    private EventInstance runningEvent;
    private bool running
    {
        get => _running;
        set
        {
           
            if (value&& !_running  )
            {
               
                    Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, SoundManager.SoundEventPaths.runPath);
                
            }
            else if( !value && _running)
            {
                
                    Client.LocalClient.SoundManagerOnline.StopEventOnGameObjectAllClients(netId,SoundManager.SoundEventPaths.runPath);

            }

            _running = value;
        }
    }

    private bool _running;

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

        float fallingDistance = 0f;

        //Jump
        if (onGround && jumping)
        {
            verticalSpeed = jumpSpeed;

            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId,SoundManager.SoundEventPaths.jumpPath);
            fallingDistance = transform.position.y;
        }

        // Apply gravity
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        displacement.y = verticalSpeed * Time.deltaTime;


        Vector3 horDisplacement = displacement.WithY(0);
        bool walking = onGround && horDisplacement.magnitude > CharacterController.minMoveDistance;
        running = walking && !Character.isDead;

        // Apply Movement to Player
        CollisionFlags collisionFlags = CharacterController.Move(displacement);

        //Apply rotation to Player
        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Process vertical collisions
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            SoundManager.SoundManagerParameter[] parameters = new SoundManager.SoundManagerParameter[1];
            //Calculates de distance of the fall
            fallingDistance = fallingDistance + transform.position.y;
            //The Max in the Clamp must be the Max range of the Event in FMOD.
            fallingDistance = Mathf.Clamp(fallingDistance, 0, 2);
            parameters[0] = new SoundManager.SoundManagerParameter("Height", fallingDistance);
            if (onGround == false)
                Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.landPath,
                    transform.position, parameters);
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
        
        if (groundLosed)
        {
            animator.SetTrigger("GroundLosed");
            groundLosed = false;
        }
    }

    private Vector3 GetDirectionRelativeToTheCamera()
    {
        Vector3 targetDirection = new Vector3(horizontalMovementInput.x, 0f, horizontalMovementInput.y);
        targetDirection =
            Character.Owner.HumanLocalPlayer.Camera.gameObject.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;
        return targetDirection.normalized;
    }

    private void OnDisable()
    {
        running = false;
    }
}