using System;
using System.Collections;
using System.Collections.Generic;
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
            _owner.Flag = this;
            SetOwnerColor();
        }
    }
    private Player _owner;

    [field: SyncVar(hook = nameof(SetNewPlayerOwner))]
    public uint OwnerNetId { get; set; }
    private void SetNewPlayerOwner(uint oldOwnerNetId, uint newOwnerNetId)
    {
        Owner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(newOwnerNetId);
    }

    [Command]
    public void CmdSetFlagActiveState(bool newVal)
    {
        _isActiveInGame = newVal;
    }
    [SyncVar(hook = nameof(NewIsActiveInGame))] 
    private bool _isActiveInGame = true;
    private void NewIsActiveInGame(bool oldVal, bool newVal)
    {
        this.gameObject.SetActive(newVal);
    }

    List<Character> charactersInTrigger = new List<Character>();
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || character != Owner.Character || character.HasFlag  ||
        charactersInTrigger.Contains(character))
        return;
        charactersInTrigger.Add(character);
        Attach();
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (character != null)
            charactersInTrigger.Remove(character);
    }

    private void Attach()
    {
        Owner.Character.CmdSetHasFlag(true); 
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.SoundEventPaths.pickUpPath,
            transform.position, null);
    }
    
    public void SetDetachPosition()
    {
        transform.position = Owner.Character.transform.position;
    }

    public void Reset()
    {
        Owner.Character.CmdSetHasFlag(false); 
        transform.position = FlagSpawner.Instance.GetSpawnPos();
    }

    public void SetOwnerColor()
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