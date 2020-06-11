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
    private bool _hasFlag;
    public bool HasFlag => _hasFlag;
    
    [Command]
    public void CmdSetHasFlag(bool value)
    {
        _hasFlag = value;
    }
    
    private void NewHasFlagValue(bool oldVal, bool newVal)
    {
        flagGameObject.SetActive(newVal); //The flag object carried by the character
        if (Owner != null && Owner.Flag != null)
            Owner.Flag.CmdSetFlagActiveState(!newVal);
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

    [field: SyncVar(hook = nameof(NewDeadState))]
    public bool isDead { get; private set; }

    private void NewDeadState(bool oldIsDead, bool newIsDead)
    {
        if (oldIsDead == true && newIsDead == false) // From dead to alive
            Debug.LogError($"Trying to revive {this.gameObject.name}.");
        else if (oldIsDead == false && newIsDead == true) // From alive to dead
            ActivateRagdoll();
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

    public void Suicide()
    {
        Kill( /*Vector3.up + transform.forward * 2, transform.position + Vector3.up*/);
    }

    public void Kill( /*Vector3 impactDirection, Vector3 impactPoint*/)
    {
        CmdDie( /*impactDirection, impactPoint*/);
        CmdSetHasFlag(false);
    }

    [Command] // From client to server
    private void CmdDie( /*Vector3 impactDirection, Vector3 impactPoint*/)
    {
        Debug.Log("CmdDie called");

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
                Owner.Flag.SetDetachPosition();
            StartCoroutine(DieCoroutine());
        }

        IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
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