using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : NetworkBehaviour
{
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public GameObject flagGameObject;

    private bool wasOnHell;
    
    [SyncVar] public Vector3 synchronizedVelocity;
    [Command]
    public void CmdSyncVelocity(Vector3 velocity)
    {
        synchronizedVelocity = velocity;
    }

    
    [Command]
    public void CmdSetHasFlag(bool value)
    {
        _hasFlag = value;
    }
    [SyncVar(hook = nameof(NewHasFlagValue))]
    private bool _hasFlag;
    private void NewHasFlagValue(bool oldVal, bool newVal)
    {
        flagGameObject.SetActive(newVal); //The flag object carried by the character
        if (hasAuthority && Owner != null && Owner.Flag != null)
            Owner.Flag.CmdSetFlagActiveState(!newVal);
    }
    public bool HasFlag => _hasFlag;

    [SerializeField] private SkinnedMeshRenderer[] meshRenderersToColor;
    
    public Player Owner
    {
        get => _owner;
        private set
        {
            if (_owner != null)
            {
                Debug.LogError("Trying to change the owner of a Character. Operation cancelled.", gameObject);
                return;
            }

            _owner = value;
            if (value != null)
            {
                value.Character = this;
                gameObject.name = "Character owned by " + value.gameObject.name;
                transform.SetParent(value.transform, true);
                if (Owner.HumanLocalPlayer != null)
                {
                    freeLookCamera.m_XAxis.m_InputAxisName = Owner.HumanLocalPlayer.name + "X";
                    freeLookCamera.m_YAxis.m_InputAxisName = Owner.HumanLocalPlayer.name + "Y";
                    freeLookCamera.gameObject.SetLayerRecursively(Owner.HumanLocalPlayer.PlayerLayer);
                }/*
                else
                {
                    Debug.LogWarning($"The HumanLocalPlayer of the owner ({Owner.name}) is null.");
                }*/
            }
            else
            {
                Debug.LogWarning("Setting a null Owner", gameObject);
                transform.SetParent(null, true);
            }
            
            UpdateColor();
        }
    }

    public void UpdateColor()
    {
        if (block == null)
            block = new MaterialPropertyBlock();

        foreach (SkinnedMeshRenderer mr in meshRenderersToColor)
        {
            block.SetColor(baseColor, Owner.Color);
            mr.SetPropertyBlock(block);
        }
    }

    private Player _owner;

    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;

    [Header("Ragdoll")] [SerializeField] private Collider mainCollider;
    [SerializeField] private Transform cameraRagdollLookAtTarget;
    [SerializeField] private GameObject[] ragdollVisualElements;

    [Header("Camera")]
    [SerializeField] public CinemachineFreeLook freeLookCamera;

    [field: SyncVar(hook = nameof(NewDeadState))]
    public bool isDead { get; private set; }

    private void NewDeadState(bool oldIsDead, bool newIsDead)
    {
        if (oldIsDead == true && newIsDead == false) // From dead to alive
            Debug.LogError($"Trying to revive {this.gameObject.name}.");
        else if (oldIsDead == false && newIsDead == true) // From alive to dead
        {
            HitParticlesManager.Instance.DisplayAt(transform.position + Vector3.up * 1.7f);
            ActivateRagdoll();
        }
        else // IDK....
            Debug.LogError($"Unexpected behaviour of the dead state of the {this.gameObject.name}");
    }

    public CharacterMovementController movementController { get; private set; }

    [field: SyncVar(hook = nameof(SetNewPlayerOwner))]
    public uint OwnerNetId { get; set; }

    private void SetNewPlayerOwner(uint oldOwnerNetId, uint newOwnerNetId)
    {
        Owner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newOwnerNetId);
    }

    private void Awake()
    {
        movementController = gameObject.GetComponentRequired<CharacterMovementController>();
    }

    private void Start()
    {
        Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(OwnerNetId,SoundManager.EventPaths.Spawn);
    }

    public void Suicide()
    {
        Kill( /*Vector3.up + transform.forward * 2, transform.position + Vector3.up*/);
    }

    public void Kill( /*Vector3 impactDirection, Vector3 impactPoint*/)
    {
        CmdDie( /*impactDirection, impactPoint*/);
    }

    [Command] // From client to server
    private void CmdDie( /*Vector3 impactDirection, Vector3 impactPoint*/)
    {
        if (isDead)
            return;

        isDead = true;

        RpcDie( /*impactDirection, impactPoint*/);
    }

    [ClientRpc] // Called on server, executed on all clients
    private void RpcDie( /*Vector3 impactDirection, Vector3 impactPoint*/)
    {
        /*MAYBE TODO: Apply impact with 'impactDirection' at 'impactPoint'*/

        if (hasAuthority)
        {
            if (HasFlag)
            {
                CmdSetHasFlag(false);
                Owner.Flag.Detach();
            }
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId,SoundManager.EventPaths.Death);
            StartCoroutine(DieCoroutine());
        }

        IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.EventPaths.DeathAnnouncement,Vector3.zero,null);

            Owner.CmdSpawnNewCharacter();
        }
    }
    
    private void ActivateRagdoll()
    {
        Vector3 currentVelocity = hasAuthority ? movementController.Rigidbody.velocity : synchronizedVelocity;
        
        // Enabling the ragdoll
        movementController.animator.enabled = false; // Automatically enables the ragdoll rigidbodies/colliders

        // Configuration of "default" character
        movementController.Rigidbody.constraints = RigidbodyConstraints.None;
        movementController.Rigidbody.isKinematic = true;
        movementController.Rigidbody.detectCollisions = false;
        movementController.enabled = false;
        mainCollider.enabled = false;
        
        // Ragdoll configuration
        Rigidbody[] ragdollRigidbodies = movementController.animator.GetComponentsInChildren<Rigidbody>();
        foreach (var ragdollRb in ragdollRigidbodies)
        {
            ragdollRb.isKinematic = false;
            ragdollRb.velocity = currentVelocity;
        }
        
        // Remove oppacity shader
        foreach (GameObject ragdollElement in ragdollVisualElements)
            ragdollElement.layer = 0;

        if (Owner != null && Owner.HumanLocalPlayer)
            freeLookCamera.LookAt = cameraRagdollLookAtTarget;
    }

    private void Update()
    {
        if (!hasAuthority)
            return;
        
        // Check if should be disabled
        if (isDead && cameraRagdollLookAtTarget.position.y <= MapBoundries.DeactivationZoneHeight)
        {
            Debug.Log($"The character {gameObject.name} of the player {Owner.name} has fallen into the void. Disabling it.", gameObject);
            gameObject.SetActive(false);
        } 
        else if (!isDead && transform.position.y <= MapBoundries.KillZoneHeight)
        {
            Debug.Log($"The character {gameObject.name} of the player {Owner.name} was caught trying to get out of the map! Killing it.", gameObject);
            Kill();
            wasOnHell = true;
        }

        if (wasOnHell && transform.position.y < MapBoundries.HeavenHeight)
        {
            float distance = MapBoundries.HeavenHeight - transform.position.y;
            Vector3 transformPosition = transform.position;
            transformPosition.y = transformPosition.y+MapBoundries.HeavenAttraction*Time.deltaTime;
            transform.position = transformPosition;
        }

    }
    

}