using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

// IMPORTANT NOTE: The flag class only stores information and makes changes to his own information.
// It is not meant to perform any operation or change in any other object

[SelectionBase]
public class Flag : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] meshRenderersToColor;
    [FormerlySerializedAs("rigidbody")] [SerializeField] private Rigidbody rb;
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlock block;
    
    public Player Owner //Player that can interact with the flag
    {
        get => _owner;
        private set
        {
            if (value == null)
            {
                Debug.LogWarning("Trying to set a null _owner. Cancelling operation.", gameObject);
                return;
            }
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
            charactersInTrigger.Contains(character) || !(MatchManager.instance.currentPhase is Battle))
            return;

        charactersInTrigger.Add(character);
        Attach();
    }

    /*private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (character != null)
            charactersInTrigger.Remove(character);
    }*/

    private void Attach()
    {
        Owner.Character.CmdSetHasFlag(true); 
        Client.LocalClient.SoundManagerOnline.PlayOneShotOnPosAllClients(SoundManager.EventPaths.PickUp,
            transform.position, null);
    }
    
    public void Detach()
    {
        
        Debug.Log($"Setting detach position {Owner.Character.transform.position} for ");
        Debug.DrawRay(Owner.Character.transform.position, Vector3.up, Owner.Color, 5f);
        charactersInTrigger.Remove(Owner.Character);
        transform.position = Owner.Character.transform.position;
        
    }

    public void Reset()
    {
        Debug.Log($"Reseting flag of {Owner.gameObject.name}");
        Owner.Character.CmdSetHasFlag(false); 
        transform.position = FlagSpawner.Instance.GetSpawnPos();
        gameObject.GetComponentRequired<Rigidbody>().velocity = Vector3.zero;
        charactersInTrigger.Remove(Owner.Character);

    }

    public void SetOwnerColor()
    {
        if (block == null)
            block = new MaterialPropertyBlock();

        foreach (SkinnedMeshRenderer mr in meshRenderersToColor)
        {
            block.SetColor(baseColor, Owner.Color);
            mr.SetPropertyBlock(block);
        }
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        // Avoid from being moved if not in battle state
        bool kinematicState = !(MatchManager.instance.currentPhase is Battle);
        if (rb.isKinematic != kinematicState)
            rb.isKinematic = kinematicState;
        
        // Check if should be reseted
        if (transform.position.y <= MapBoundries.DeactivationZoneHeight)
        {
            Debug.Log($"The flag of the player {Owner.name} has fallen into the void. Resetting it", gameObject);
            Reset();
        }
    }
    
}