using System;
using Mirror;
using UnityEditor.UIElements;
using UnityEngine;

// IMPORTANT NOTE: The flag class only stores information and makes changes to his own information.
// It is not meant to perform any operation or change in any other object

public class Flag : NetworkBehaviour
{

    [SerializeField] private MeshRenderer[] meshRenderersToColor;
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;

   

    public Player Owner //Player that can interact with the flag
    {
        get => _owner;
         private set
        {
            _owner = value;
            UpdateColor();
        }
    } 

    private Player _owner;

    public bool hasCarrier //Currently carrying the flag?
    {
        get => _carrier;
        private set
        {
            if (value == _carrier) return;
            if (!_carrier) Owner.Character.hasFlag = false;
            _carrier = value;
            if (_carrier) Owner.Character.hasFlag = true;
        }
    }

    [field: SyncVar(hook = nameof(SetNewPlayerOwner))]
    public uint OwnerNetId { get; set; }

    private void SetNewPlayerOwner(uint oldOwnerNetId, uint newOwnerNetId)
    {
        Owner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newOwnerNetId);
    }

    private bool _carrier;

    private void OnCollisionEnter(Collision other)
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        if (other.GetComponent<DeadZone>()) //If the flag falls off the map, reset it
        {
            if (hasAuthority)
                Reset();
            return;
        }
        if (hasCarrier  || !isServer) return;
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead ||character != Owner.Character)
            return;
        Attach();
    }

    private void Attach()
    {
        hasCarrier = true;
        transform.rotation = Quaternion.Euler(0f,0,0f);
        Owner.Character.hasFlag = true;
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.pickUpPath,
            transform.position, null);
        Debug.Log("The player "+Owner.name+" has the "+Owner.Color+" flag");
        
        this.gameObject.SetActive(false);
    }

    public void Detach(Vector3 droppedPos)
    {
        hasCarrier = false;
        transform.position = droppedPos;
        GetComponent<Rigidbody>().isKinematic = false;

        // this.gameObject.SetActive(true); This cannot be done cuz it's unactive.
        //It gets activated in the hook field of Character HasFlag
    }

    public void Reset()
    {
        Debug.Log("Reset flag");
        hasCarrier = false;
        transform.position = FlagSpawner.Instance.GetSpawnPos();
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
}