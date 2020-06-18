using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Mirror;
using Telepathy;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public abstract class Trap : NetworkBehaviour
{
    [SerializeField] private float cooldownTime = 5f;

    [FormerlySerializedAs("durationTime")] [SerializeField] private float activeTime = 3f; // must be greater than cooldownTime
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
            if (value == _isHighlighted)
                return;
            
            if (value)
            {
                MaterialPropertyBlock bundle = new MaterialPropertyBlock();
                bundle.SetColor("HIGHLIGHT_COLOR", GetHighlightColor() );
                
                foreach (MeshRenderer mr in myMeshes)
                    mr.SetPropertyBlock(bundle);
                
                foreach (MeshRenderer mr in myMeshes)
                {
                    mr.material.EnableKeyword("IS_HIGHLIGHTED");

                }
            }
            
            else if (!value)
                foreach (MeshRenderer mr in myMeshes)
                    mr.material.DisableKeyword("IS_HIGHLIGHTED");

            _isHighlighted = value;
        }
    }

    private static Color GetHighlightColor()
    {
        Color color = Color.white;
        if (Client.LocalClient.PlayersManager.players.Count == 1)
            foreach (Player player in Client.LocalClient.PlayersManager.players)
                color = player.Color;
        return color;
    }

    private bool _isHighlighted;

    private void Update()
    {
        if (isServer)
            ServerUpdate();
        
        if (isVisible)
        {
            if (dissolvePercent > 0)
            {
                dissolvePercent -= dissolveDuration*Time.deltaTime;
            }
        }

        if (!isVisible)
        {
            if (dissolvePercent < 1)
            {
                dissolvePercent += dissolveDuration*Time.deltaTime;
            }
        }

        dissolvePercent = Mathf.Clamp01(dissolvePercent);
         
        MaterialPropertyBlock bundle = new MaterialPropertyBlock();
        bundle.SetFloat( DissolvePercent, dissolvePercent);
        foreach (MeshRenderer mr in myMeshes)
            mr.SetPropertyBlock(bundle);

    }

    private float dissolvePercent; 
    [SerializeField] private float dissolveDuration = 1f;
    private static readonly int DissolvePercent = Shader.PropertyToID("DISSOLVE_PERCENT");

    private bool isVisible  => isActive || !OnCd;
    private bool reloaded;
    
    [Server]
    private void ServerUpdate()
    {
        if (OnCd) cdTimer -= Time.deltaTime;
        if (isActive)
        {
            activatedTimer -= Time.deltaTime;
            reloaded = false;
        }
        else
        {
            if (dissolvePercent >=1 && !reloaded)
            {
                Reload();
            }
        }
        
    }

    [ContextMenu("Reload")]
    [Server]
    protected virtual void Reload()
    {
        reloaded = true;
        Debug.Log($"The trap '{gameObject.name}' is being reloaded.", this.gameObject);
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
        activatedTimer = activeTime;

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