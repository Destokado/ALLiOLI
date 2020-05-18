using System.Collections.Generic;
using Mirror;
using Telepathy;
using UnityEngine;

public abstract class Trap : NetworkBehaviour
{
    [SerializeField] private float cooldownTime = 5f;

    [SerializeField] private float durationTime = 3f; // must be greater than the cdTimer
    [SerializeField] protected RadarTriggerTrap radarTrigger;
    
    public bool OnCd => cdTimer > 0;
    [field: SyncVar] public float cdTimer { get; private set; }
    
    public bool active => activatedTimer > 0;
    [field: SyncVar] public float activatedTimer { get; private set; }

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }
    
    [Server]
    private void ServerUpdate()
    {
        if (OnCd) cdTimer -= Time.deltaTime;

        if (active)
        {
            activatedTimer -= Time.deltaTime;
            if (!active) Reload();
        }
    }

    [Server]
    protected virtual void Reload() { }

    [Server]
    public virtual void Activate()
    {
        Debug.Log("The trap '" + gameObject.name + "' has been activated.", this);

        cdTimer = cooldownTime;
        activatedTimer = durationTime;
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