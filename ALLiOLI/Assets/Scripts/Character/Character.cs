using System;
using System.Collections;
using Mirror;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : NetworkBehaviour
{
    [SerializeField] public Transform cameraTarget;
    [SerializeField] public Transform interactionRayOrigin;

    [HideInInspector] public Flag flag;
    [SerializeField] public Transform flagPosition;


    [SerializeField] private MeshRenderer[] meshRenderersToColor;

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
        
        foreach (MeshRenderer mr in meshRenderersToColor)
        {
            block.SetColor(baseColor, Owner.Color);
            mr.SetPropertyBlock(block);
        }
    }

    private Player _owner;
    
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;

    public bool isDead { get; private set; }

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

    [ClientRpc]
    public void RpcDie(Vector3 impact, Vector3 impactPoint)
    {
        if (!isDead)
            StartCoroutine(DieCoroutine(impact, impactPoint));
    }

    private IEnumerator DieCoroutine(Vector3 impact, Vector3 impactPoint)
    {
        Debug.Log("CHAR DYING: " + gameObject.name);
        isDead = true;
        if (flag != null) flag.Detach();
        movementController.enabled = false;

        movementController.enabled = false;
        movementController.CharacterController.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForceAtPosition(impact, impactPoint, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);

        if (hasAuthority)
            Owner.CmdSpawnNewCharacter();
    }

    [Command] // From a client to the server
    public void CmdServerSuicide()
    {
        ServerSuicide();
    }

    [Server]
    public void ServerSuicide()
    {
        RpcDie(Vector3.up + transform.forward * 2, transform.position + Vector3.up);
    }
}