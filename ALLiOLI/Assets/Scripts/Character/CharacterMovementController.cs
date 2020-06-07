using System;
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
    [Space] [SerializeField] private Animator animator;

    [SerializeField] private float walkingStateVelocityThreshold = 0.01f;
    
    [NonSerialized] public Vector2 horizontalMovementInput;
    
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundedLayers;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float airForce = 1f;


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

    public void Jump()
    {
        Rigidbody.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
        Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId,SoundManager.SoundEventPaths.jumpPath);
    }

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
    private float turnSmoothVelocity;


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

        bool wantsToMove = desiredDisplacement.magnitude > walkingStateVelocityThreshold;
        walking = onGround && wantsToMove;
        
        if (walking) // On ground and wants to move
            Rigidbody.velocity = desiredDisplacement.WithY(Rigidbody.velocity.y);
        
        else if (onGround) // On ground and does not want to move
            Rigidbody.velocity = Vector3.zero.WithY(Rigidbody.velocity.y);
        
        else if (wantsToMove) // On air and wants to move
        {
            Vector2 rigidbodyHorVelocity = Rigidbody.velocity.ToVector2WithoutY();
            Vector2 desiredHorDisplacement = desiredDisplacement.ToVector2WithoutY();
            float similarityBetweenVelocityAndDesiredDisplacement = Vector2.Dot(rigidbodyHorVelocity.normalized,desiredHorDisplacement.normalized)*-1f/2f+0.5f;
            Vector3 force = desiredDisplacement * (airForce * similarityBetweenVelocityAndDesiredDisplacement);
            Rigidbody.AddForce(force, ForceMode.Acceleration);
            Debug.DrawRay(transform.position, rigidbodyHorVelocity.normalized, Color.white);
            Debug.DrawLine(transform.position, transform.position+force, Color.black);
            /*float GetAirDisplacementResult(float currentVelocity, float desiredVelocity)
            {
                float tempVel = currentVelocity;
                
                if ((currentVelocity < desiredVelocity && desiredVelocity > 0) || // Wants to increase
                    (currentVelocity > desiredVelocity && desiredVelocity < 0)) // Wants to decrease
                {
                    tempVel += desiredVelocity * maxAirControl;
                }

                return tempVel;
            }
            
            Vector3 rigidbodyVelocity = Rigidbody.velocity;
            Vector2 tempHorVel = new Vector2(GetAirDisplacementResult(rigidbodyVelocity.x, desiredDisplacement.x), GetAirDisplacementResult(rigidbodyVelocity.z, desiredDisplacement.z));
            if (tempHorVel.magnitude > maxAirSpeed)
            {
                Debug.Log($"TOO HIGH {tempHorVel.magnitude}");
                tempHorVel = tempHorVel.normalized * maxAirSpeed;
            }
            else
            {
                Debug.Log($"normal {tempHorVel.magnitude}");
            }
            Rigidbody.velocity = new Vector3(tempHorVel.x, rigidbodyVelocity.y, tempHorVel.y);*/
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

        // Process vertical collisions
        onGround = Physics.OverlapSphere(groundCheckPosition.position, 0.1f, groundedLayers).Length > 0;

        Debug.DrawRay(groundCheckPosition.position, Vector3.up, onGround? Color.cyan : Color.gray);
        
        GiveStateToAnimations(desiredDisplacement);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = onGround? Color.cyan : Color.gray;
        Gizmos.DrawWireSphere(groundCheckPosition.position, 0.1f);
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