using System;
using Mirror;
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
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KillZone>())
        {
            if (hasAuthority)
                Reset();

            return;
        }

        if (hasCarrier  || !isServer) return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || character != _owner.Character)
            return;

        Attach();
    }

    private void Attach()
    {
       
        hasCarrier = true;
        Owner.Character.hasFlag = true;
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.pickUpPath,
            transform.position, null);
        Debug.Log("The player "+Owner.name+" has the "+Owner.Color+" flag");
        this.gameObject.SetActive(false);

    }

    public void Detach()
    {
        hasCarrier = false;
        Owner.Character.hasFlag = false;
       // this.gameObject.SetActive(true); This cannot be done cuz it's unactive.
       //It gets activated in the ServerDie of Character


    }

    public void Reset()
    {
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