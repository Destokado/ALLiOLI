using System;
using System.Collections;
using Mirror;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : NetworkBehaviour
{
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public GameObject flagGameObject;
    
    [SyncVar(hook = nameof(NewHasFlagValue))] public bool hasFlag = false;
    private void NewHasFlagValue(bool oldVal, bool newVal) { flagGameObject.SetActive(newVal); }

    [SerializeField] private SkinnedMeshRenderer[] meshRenderersToColor;
    
    public Player Owner
    {
        get => _owner;
        private set
        {
            if (_owner != null)
            {
                Debug.LogError("Trying to change the owner of a Character. Operation cancelled.");
                return;
            }
            
            _owner = value;
            value.Character = this;
            gameObject.name = "Character owned by " + value.gameObject.name;
            transform.SetParent(value.transform, true);
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
    
    [Header("Ragdoll")]
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Transform cameraRagdollLookAtTarget;

    [field: SyncVar] public bool isDead { get; private set; }

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

    [Command] // From a client to the server
    public void CmdServerSuicide()
    {
        ServerSuicide();
    }

    [Server] // On the server
    public void ServerSuicide()
    {
        ServerDie(Vector3.up + transform.forward * 2, transform.position + Vector3.up);
    }

    public void SafeServerDie(Vector3 impact, Vector3 impactPoint)
    {
        if (!isServer)
        {
            string callingMethod = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name;
            Debug.LogError($"Calling 'SafeServerDie' from an object that is not in a server. Called from method {callingMethod}. Dead impact happened at {impactPoint}.");
        }
        
        ServerDie(impact, impactPoint);
    }
    
    [Server]
    public void ServerDie(Vector3 impact, Vector3 impactPoint)
    {
        if (isDead)
            return;
        
        isDead = true;
        if (hasFlag) 
            FlagManager.Instance.FlagDropped(flagGameObject.transform.position);
        
        RpcDie(impact, impactPoint);
    }

    [ClientRpc] // Called on server, executed on all clients
    private void RpcDie(Vector3 impact, Vector3 impactPoint)
    {
        StartCoroutine(DieCoroutine());

        IEnumerator DieCoroutine()
        {
            ActivateRagdoll();

            yield return new WaitForSeconds(1.5f);

            if (hasAuthority)
                Owner.CmdSpawnNewCharacter();
        }
    }

    [ContextMenu("Enable Ragdoll")]
    private void ActivateRagdoll()
    {
            movementController.animator.enabled = !true; // Automatically enables the ragdoll rigidbodies/colliders
            
            movementController.Rigidbody.constraints = true? RigidbodyConstraints.None : RigidbodyConstraints.FreezeRotation;
            movementController.Rigidbody.isKinematic = true;
            movementController.Rigidbody.detectCollisions = !true;
            
            movementController.enabled = !true;
            
            mainCollider.enabled = !true;
            
            Owner.HumanLocalPlayer.Camera.ForceSetLookAt(cameraRagdollLookAtTarget);
    }

}