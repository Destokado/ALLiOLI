using System.Collections;
using Mirror;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : NetworkBehaviour
{
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public GameObject flagGameObject;
    [SerializeField] public MeshRenderer flagMeshToColor;

   [SyncVar(hook = nameof(NewHasFlagValue))]
    public bool hasFlag ;

    private void NewHasFlagValue(bool oldVal, bool newVal)
    {
        flagGameObject.SetActive(newVal); //The gameobject of the placeholder flag
        if(newVal)  NetworkServer.UnSpawn(Owner.Flag.gameObject); //The character picks the flag
        else
        {
            Owner.Flag.gameObject.SetActive(true);
            NetworkServer.Spawn(Owner.Flag.gameObject,Owner.connectionToClient);
            Owner.Flag.Detach(transform.position);
        }

    }

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
        
        flagMeshToColor.SetPropertyBlock(block);
    }

    private Player _owner;

    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;

    [Header("Ragdoll")] [SerializeField] private Collider mainCollider;
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
    
    public void Suicide()
    {
        Kill(Vector3.up + transform.forward * 2, transform.position + Vector3.up);
    }

    public void Kill(Vector3 impactDirection, Vector3 impactPoint)
    {
        CmdDie(impactDirection, impactPoint);
    }

    [Command] // From client to server
    private void CmdDie(Vector3 impactDirection, Vector3 impactPoint)
    {
        hasFlag = false;
        
        if (isDead)
            return;
        
        isDead = true;

        RpcDie(impactDirection, impactPoint);
    }

    [ClientRpc] // Called on server, executed on all clients
    private void RpcDie(Vector3 impactDirection, Vector3 impactPoint)
    {
        StartCoroutine(DieCoroutine());

        IEnumerator DieCoroutine()
        {
            ActivateRagdoll();
            // TODO: Apply impact

            yield return new WaitForSeconds(1.5f);

            if (hasAuthority)
                Owner.CmdSpawnNewCharacter();
        }
    }

    [ContextMenu("Enable Ragdoll")]
    private void ActivateRagdoll()
    {
        movementController.animator.enabled = !true; // Automatically enables the ragdoll rigidbodies/colliders

        movementController.Rigidbody.constraints =
            true ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeRotation;
        movementController.Rigidbody.isKinematic = true;
        movementController.Rigidbody.detectCollisions = !true;

        movementController.enabled = !true;

        mainCollider.enabled = !true;

        if (Owner.HumanLocalPlayer)
            Owner.HumanLocalPlayer.Camera.ForceSetLookAt(cameraRagdollLookAtTarget);
    }
}