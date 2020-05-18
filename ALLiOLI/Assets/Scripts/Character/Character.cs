using System.Collections;
using Mirror;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovementController))]
public class Character : NetworkBehaviour
{
    [SerializeField] public Transform cameraTarget;

    [HideInInspector] public Flag flag;
    [SerializeField] public Transform flagPosition;
    public Player Owner
    {
        get => _owner;
        private set
        {
            _owner = value;
            value.Character = this;
            gameObject.name = "Character owned by " + value.gameObject.name;
        }
    }
    private Player _owner;

    [field: SyncVar(hook = nameof(SetPlayerSpawnerAsOwner))]
    public uint PlayerSpawnerNetId { get; set; }

    public bool isDead { get; private set; }
    
    public CharacterMovementController movementController { get; private set; }

    private void SetPlayerSpawnerAsOwner(uint oldPlayerNetId, uint newPlayerNetId)
    {
        if (!NetworkIdentity.spawned.ContainsKey(newPlayerNetId))
        {
            Debug.LogWarning("Player spawner with NetId " + newPlayerNetId + " not found.");
            return;
        }
        
        Owner = NetworkIdentity.spawned[newPlayerNetId].gameObject.GetComponent<Player>();
    }

    private void Awake()
    {
        movementController = gameObject.GetComponentRequired<CharacterMovementController>();
    }

    public void Die(Vector3 impact, Vector3 impactPoint)
    {
        if (!isDead)
            StartCoroutine(DieCoroutine(impact, impactPoint));
    }

    private IEnumerator DieCoroutine(Vector3 impact, Vector3 impactPoint)
    {
        isDead = true;
        if (flag != null) flag.Detach();
        movementController.enabled = false;

        movementController.enabled = false;
        movementController.CharacterController.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForceAtPosition(impact, impactPoint, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);

        //TODO: Respawn
        //Owner.CmdSpawnNewCharacter();
    }

    public void Suicide()
    {
        Die(Vector3.up * 2, transform.position);
    }
}