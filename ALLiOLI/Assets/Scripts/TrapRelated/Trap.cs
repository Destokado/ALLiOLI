using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class Trap : NetworkBehaviour
{
    [SerializeField] private float cooldownTime = 5f;

    [SerializeField] private float durationTime = 3f; // must be greater than the cdTimer
    [SerializeField] protected RadarTriggerTrap radarTrigger;
    public bool OnCd => cdTimer > 0;
    public float cdTimer { get; private set; }
    public bool active => activatedTimer > 0;
    public float activatedTimer { get; private set; }

    private void Update()
    {
        if (OnCd) cdTimer -= Time.deltaTime;

        if (active)
        {
            activatedTimer -= Time.deltaTime;
            if (!active) Reload();
        }
    }

    protected virtual void Reload() { }

    [ClientRpc]
    public virtual void RpcActivate()
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