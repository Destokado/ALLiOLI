using System;
using FMOD;
using FMOD.Studio;
using Mirror;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
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
    [NonSerialized] public bool groundLosed;
    
    private float verticalSpeed;

    [Header("Configuration")] [SerializeField]
    private float walkSpeed;

    public bool jumpTrigger { get; set; } // TODO: on set as true, apply force and automatically set as false (if grounded)
    //TODO: play this sound when triggered the jump: Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId,SoundManager.SoundEventPaths.jumpPath);

    private bool walking;
    
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

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = gameObject.GetComponentRequired<Rigidbody>();
            return _rigidbody;
        }
        private set { if (_rigidbody) Debug.LogWarning($"Trying to reset the rigidbody of the character {Character.gameObject.name} owned by {Character.Owner.gameObject.name}.", gameObject); _rigidbody = value; }
    }
    private Rigidbody _rigidbody;
    
    public Character Character
    {
        get
        {
            if (_character == null)
                _character = gameObject.GetComponentRequired<Character>();
            return _character;
        }
        private set { if (_character) Debug.LogWarning($"Trying to reset the rigidbody of {gameObject.name}.", gameObject); _character = value; }
    }
    private Character _character;
    
    [Header("Rotation")]
    [SerializeField] private float turnSmoothTime = .1f;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float walkingStateVelocityThreshold = 0.01f;


    private void Awake()
    {
        
      
        Character = gameObject.GetComponentRequired<Character>();
    }

    private void FixedUpdate()
    {
        if (hasAuthority)
            AuthorityFixedUpdate();
    }

    
    private void AuthorityFixedUpdate()
    {
        Vector3 direction = GetDirectionRelativeToTheCamera();
        Vector3 desiredDisplacement = direction * (walkSpeed * Time.deltaTime); // TODO: account for the ground's normal if grounded
        walking = onGround && desiredDisplacement.magnitude > walkingStateVelocityThreshold && !Character.isDead;
        if (walking)
            Rigidbody.velocity = desiredDisplacement;
        else if (onGround)
            Rigidbody.velocity = Vector3.zero;

        //Apply rotation to Player
        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Process vertical collisions
        if (true) // TODO: if (checkIfGrounded() == true)
        {
            onGround = true;
        }
        else
        {
            onGround = false; // DO NOT REMOVE while the 'TODO' in "Process vertical collisions" still there
        }

        GiveStateToAnimations(desiredDisplacement);
    }

    private void PlaySoundFall(float fallingDistance) // TODO: Re add where needed
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
    }

    private void GiveStateToAnimations(Vector3 displacement)
    {
        animator.SetFloat("HorMove", Mathf.Abs(displacement.ToVector2WithoutY().magnitude) * 10);
        animator.SetFloat("VerMove", Mathf.Abs(displacement.y) * 10);
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