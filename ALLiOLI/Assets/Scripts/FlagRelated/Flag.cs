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
            value.Flag = this;
            UpdateColor();
        }
    }

    private Player _owner;

    [field: SyncVar(hook = nameof(SetNewPlayerOwner))]
    public uint OwnerNetId { get; set; }

    private void SetNewPlayerOwner(uint oldOwnerNetId, uint newOwnerNetId)
    {
        Owner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newOwnerNetId);
    }

    private bool _carrier;

    [SyncVar(hook = nameof(NewIsActiveInGame))] [NonSerialized]
    public bool isActiveInGame = true;

    private void NewIsActiveInGame(bool oldVal, bool newVal)
    {
        this.gameObject.SetActive(newVal);
        if (newVal) transform.position = Owner.Character.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || character != Owner.Character || character.hasFlag)
            return;
        Attach();
    }

    private void Attach()
    {
        Owner.Character.hasFlag = true;
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.pickUpPath,
            transform.position, null);
        Debug.Log("The player " + Owner.name + " has the " + Owner.Color + " flag");
    }

    public void Reset()
    {
        if (!hasAuthority) return;
        
        Owner.Character.hasFlag = false;
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