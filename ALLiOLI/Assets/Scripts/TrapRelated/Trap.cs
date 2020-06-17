using System;
using System.Collections.Generic;
using FMODUnity;
using Mirror;
using Telepathy;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public abstract class Trap : NetworkBehaviour
{
    [SerializeField] private float cooldownTime = 5f;

    [SerializeField] private float durationTime = 3f; // must be greater than the cdTimer
    [SerializeField] protected RadarTriggerTrap radarTrigger;
    [SerializeField] protected StudioEventEmitter activateEmitter;
    [SerializeField] private Material myMaterial;

    public bool OnCd => cdTimer > 0;
    [field: SyncVar] public float cdTimer { get; private set; }
    public bool isActive => activatedTimer > 0;
    [field: SyncVar] public float activatedTimer { get; private set; }


    protected virtual void Awake()
    {
        myMaterial = GetComponentInChildren<MeshRenderer>().material;
    }

    public bool isHighlighted
    {
        get => _isHighlighted;
        set
        {
            if (value) myMaterial.EnableKeyword("IS_HIGHLIGHTED");
            if (!value) myMaterial.DisableKeyword("IS_HIGHLIGHTED");
            _isHighlighted = value;
        }
    }

    private bool _isHighlighted;

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }

    private bool isEnabled
    {
        set
        {
            if (value) myMaterial.EnableKeyword("IS_DISABLED"); //TODO: WHEN !Active && OnCd
            if (!value) myMaterial.DisableKeyword("IS_DISABLED");
        }
    }

    [Server]
    private void ServerUpdate()
    {
        if (OnCd) cdTimer -= Time.deltaTime;
        if (isActive)
        {
            activatedTimer -= Time.deltaTime;
            if (!isActive) Reload();
        }

        if (!isActive && OnCd) isEnabled = false;
        else
        {
            isEnabled = false;
        }
    }

    [ContextMenu("Reload")]
    [Server]
    protected virtual void Reload()
    {
        Debug.Log($"The trap '{gameObject.name}' is being deactivated. Reloading.", this.gameObject);
    }

    [ContextMenu("Activate")]
    [Server]
    public virtual void Activate()
    {
        if (activateEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, activateEmitter.Event);
        else if (activateEmitter.Event.IsNullOrEmpty())
        {
            Debug.LogWarning($" The activateEmitter.event is Empty  in {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($" The activateEmitter is null in {gameObject.name}");
        }

        cdTimer = cooldownTime;
        activatedTimer = durationTime;

        Debug.Log($"The trap '{gameObject.name}' is being activated.", this.gameObject);
    }

    public bool IsActivatable()
    {
        return !OnCd;
    }

    public float GetDistanceTo(Character character)
    {
        return radarTrigger.GetRadarDistanceTo(character);
    }

    public SortedList<float, Character> GetCharactersInTrapRadar(Player exception)
    {
        return radarTrigger.GetCharactersInTrapRadar(exception);
    }
}