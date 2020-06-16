using System;
using System.Linq;
using FMOD;
using FMOD.Studio;
using Mirror;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Character))]
public class CharacterMovementController : NetworkBehaviour
{
    
    [NonSerialized] public Vector2 horizontalMovementInput;

    [Header("Animation")] 
    [SerializeField] private Animator _animator;
    [SerializeField] public Animator animator => _animator;
    [SerializeField] public NetworkAnimator networkAnimator;

    [Header("Thresholds")] 
    [Tooltip("Minimum velocity to declare that the character wants to move intentionally by the human.")]
    [SerializeField] private float voluntaryMovementStateThreshold = 0.01f;
    [Tooltip("Minimum horizontal velocity to apply forces (and be able to move) while the character is in air (not grounded).\nUseful to avoid getting stuck in walls while sliding next to them.")]
    [SerializeField] private float airMovementThreshold = 0.1f;
    
    [Header("Movement configuration")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movingForceOnAir = 3f;
    
    [Header("Environment configuration")]
    [SerializeField] private LayerMask groundedLayers;
    [SerializeField] private Transform[] groundCheckPositions;
    [SerializeField] private float groundCheckRadius = 0.1f;
    
    public bool onGround
    {
        get => _onGround;
        private set
        {
            if (value != _onGround)
            {
                if (value == false)
                    setAnimGroundLosed = true;
                _onGround = value;
            }
        }
    }
    // ReSharper disable once InconsistentNaming
    private bool _onGround;
    private bool setAnimGroundLosed;
    private bool walking;
    
    [Header("Rotation")]
    [SerializeField] private float turnSmoothTime = .1f;
    private float turnSmoothVelocity;

    private EventInstance runningEvent;
    private bool running
    {
        get => _running;
        set
        {
            if (value&& !_running )
                Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, SoundManager.EventPaths.Run);
            else if( !value && _running)
                Client.LocalClient.SoundManagerOnline.StopEventOnGameObjectAllClients(netId,SoundManager.EventPaths.Run);

            _running = value;
        }
    }
    // ReSharper disable once InconsistentNaming
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
    // ReSharper disable once InconsistentNaming
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
    // ReSharper disable once InconsistentNaming
    private Character _character;
    



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
        if (Character.isDead)
            return;

        Vector3 direction = GetDirectionRelativeToTheCamera();
        Vector3 desiredDisplacement = direction * (walkSpeed * Time.deltaTime);

        bool wantsToMove = desiredDisplacement.magnitude > voluntaryMovementStateThreshold;
        walking = onGround && wantsToMove;
        running = walking;
        if (walking) // On ground and wants to move
            Rigidbody.velocity = desiredDisplacement.WithY(Rigidbody.velocity.y);
        
        else if (onGround) // On ground and does not want to move
            Rigidbody.velocity = Vector3.zero.WithY(Rigidbody.velocity.y);
        
        else if (wantsToMove) // On air and wants to move
        {
            if (Rigidbody.velocity.ToVector2WithoutY().magnitude > airMovementThreshold)
            {
                Vector2 rigidbodyHorVelocity = Rigidbody.velocity.ToVector2WithoutY();
                Vector2 desiredHorDisplacement = desiredDisplacement.ToVector2WithoutY();
                float similarityBetweenVelocityAndDesiredDisplacement = Vector2.Dot(rigidbodyHorVelocity.normalized,desiredHorDisplacement.normalized)*-1f/2f+0.5f;
                Vector3 force = desiredDisplacement * (movingForceOnAir * similarityBetweenVelocityAndDesiredDisplacement);
                Rigidbody.AddForce(force, ForceMode.Acceleration);
                Debug.DrawLine(transform.position, transform.position+(force/10f), Color.black);
            }
        }
        
        //Apply rotation to Player
        Vector3 rigidbodyVelocity = Rigidbody.velocity;
        if (rigidbodyVelocity.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(rigidbodyVelocity.x, rigidbodyVelocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        bool tempOnGround = false;
        // Check if is grounded
        foreach (Transform checkTransform in groundCheckPositions)
        {
            tempOnGround = CheckIfTouchesGround(checkTransform.position);
            if (tempOnGround) break;
        }

        Vector3 fallStartPos = Vector3.zero;
        if (!tempOnGround && onGround) //It just stopped being grounded
        {
            fallStartPos = transform.position;
        }
        if (tempOnGround && !onGround) //It joust touched the ground
        {
            
            PlaySoundFall(Mathf.Abs(fallStartPos.y-transform.position.y));
        }
        
        onGround = tempOnGround;

        Debug.DrawRay(transform.position, Vector3.up, onGround? Color.cyan : Color.gray);
        Debug.DrawRay(transform.position, Rigidbody.velocity/10f, Color.white);

        GiveStateToAnimations(Rigidbody.velocity);
        
        //Synchronize the velocity
        Character.CmdSyncVelocity(Rigidbody.velocity);
    }

    private bool CheckIfTouchesGround(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, groundCheckRadius, groundedLayers);
        return colliders.Any(collider => !collider.isTrigger);
    }

    public void Jump()
    {
        if(!onGround) return;
        Rigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        // TODO: avoid jump sound if the jump has not been performed properly (the character got stucked in a wall)
        Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId,SoundManager.EventPaths.Jump); 
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Transform checkTransform in groundCheckPositions)
        {
            Gizmos.color = CheckIfTouchesGround(checkTransform.position)? Color.cyan : Color.gray;
            Gizmos.DrawWireSphere(checkTransform.position, groundCheckRadius);
        }
    }

    private void PlaySoundFall(float fallingDistance) // TODO: Re add where needed
    {
        SoundManager.SoundManagerParameter[] parameters = new SoundManager.SoundManagerParameter[1];
        //Calculates de distance of the fall
        fallingDistance = fallingDistance + transform.position.y;
        //The Max in the Clamp must be the Max range of the Event in FMOD.
        fallingDistance = Mathf.Clamp(fallingDistance, 0, 5);
        parameters[0] = new SoundManager.SoundManagerParameter("Distance", fallingDistance);
        if (onGround == false)
            Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.EventPaths.Land,
                transform.position, parameters);
    }

    private void GiveStateToAnimations(Vector3 displacement)
    {
        animator.SetFloat("HorMove", Mathf.Abs(displacement.ToVector2WithoutY().magnitude) * 10);
        animator.SetFloat("VerMove", Mathf.Abs(displacement.y) * 10);
        animator.SetBool("Grounded", onGround);
        if (setAnimGroundLosed)
        {
            networkAnimator.SetTrigger("GroundLosed");
            setAnimGroundLosed = false;
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