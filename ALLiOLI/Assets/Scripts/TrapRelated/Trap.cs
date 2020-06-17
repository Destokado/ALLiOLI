using System;
using System.Collections.Generic;
using System.Linq;
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

    public bool OnCd => cdTimer > 0;
    [field: SyncVar] public float cdTimer { get; private set; }
    public bool isActive => activatedTimer > 0;
    [field: SyncVar] public float activatedTimer { get; private set; }
    
    [NonSerialized] private List<MeshRenderer>myMeshes = new List<MeshRenderer>();
    
    protected virtual void Awake()
    {
        myMeshes = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        
        MeshRenderer m = GetComponent<MeshRenderer>();
        if (m != null && !myMeshes.Contains(m)) 
            myMeshes.Add(m);
    }

    public bool isHighlighted
    {
        get => _isHighlighted;
        set
        {

            if(value)
                foreach (MeshRenderer mr in myMeshes)
                    mr.material.EnableKeyword("IS_HIGHLIGHTED");
            
            else if (!value)
                foreach (MeshRenderer mr in myMeshes)
                    mr.material.DisableKeyword("IS_HIGHLIGHTED");

            _isHighlighted = value;
        }
    }

    private bool _isHighlighted;

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }

    private float dissolvePercent;
    private float dissolveSpeed = .01f;
    private static readonly int DissolvePercent = Shader.PropertyToID("DISSOLVE_PERCENT");

    private bool isEnabled  => !isActive && OnCd;
    
    [Server]
    private void ServerUpdate()
    {
        if (OnCd) cdTimer -= Time.deltaTime;
        if (isActive)
        {
            activatedTimer -= Time.deltaTime;
            if (!isActive) Reload();
        }

        if (isEnabled)
        {
            if (dissolvePercent < 1)
            {
                dissolvePercent += dissolveSpeed;
            }
        }

        if (!isEnabled)
        {
            if (dissolvePercent > 0)
            {
                dissolvePercent -= dissolveSpeed;
            }
        }

         dissolvePercent = Mathf.Clamp01(dissolvePercent);
         
         MaterialPropertyBlock bundle = new MaterialPropertyBlock();
         foreach (MeshRenderer mr in myMeshes)
         {
             bundle.SetFloat( DissolvePercent,dissolvePercent);
             mr.SetPropertyBlock(bundle);
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