using System;
using Mirror;
using UnityEngine;

// IMPORTANT NOTE: The flag class only stores information and makes changes to his own information.
// It is not meant to perform any operation or change in any other object

public class Flag : NetworkBehaviour
{

    [SerializeField] private SkinnedMeshRenderer[] meshRenderersToColor;
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;

    private Color color
    {
        set
        {
            if (block == null)
                block = new MaterialPropertyBlock();

            foreach (SkinnedMeshRenderer mr in meshRenderersToColor)
            {
                block.SetColor(baseColor, owner.Color);
                mr.SetPropertyBlock(block);
            }
        }
    }

    public Player owner
    {
        get => _owner;
        set
        {
            _owner = value;
            color = _owner.Color;
        }
    } //Player that can interact with the flag

    private Player _owner;

    public bool hasCarrier //Currently carrying the flag?
    {
        get => _carrier;
        private set
        {
            if (value == _carrier) return;
            if (!_carrier) owner.Character.hasFlag = false;
            _carrier = value;
            if (_carrier) owner.Character.hasFlag = true;
        }
    }

    [field: SyncVar(hook = nameof(SetNewPlayerOwner))]
    public uint OwnerNetId { get; set; }

    private void SetNewPlayerOwner(uint oldOwnerNetId, uint newOwnerNetId)
    {
        owner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newOwnerNetId);
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

        if (hasCarrier  || isServer) return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || character != _owner.Character)
            return;

        Attach();
    }

    private void Attach()
    {
        hasCarrier = true;
        Debug.Log("The player "+owner.name+" has the "+owner.Color+" flag");
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.pickUpPath,
            transform.position, null);
    }

    public void Detach()
    {
        hasCarrier = false;
        
    }

    public void Reset()
    {
        hasCarrier = false;
        transform.position = FlagSpawner.Instance.GetSpawnPos();
    }
}