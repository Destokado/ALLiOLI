using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private RadarTriggerTrap radarTrigger;
    public bool OnCd => cdTimer > 0;
    public float cdTimer { get; private set; }

    private void Update()
    {
        if (OnCd) {
            cdTimer -= Time.deltaTime;
            if (!OnCd)
                Reload();
        }
    }

    protected abstract void Reload();

    public virtual void Activate()
    {
        Debug.Log("The trap "+gameObject.name+" has been activated.");
        cdTimer = cooldownTime;
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