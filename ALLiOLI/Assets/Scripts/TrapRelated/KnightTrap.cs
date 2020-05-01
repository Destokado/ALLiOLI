using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class KnightTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private KillZone killZone;

    public void Awake() 
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
        killZone.SetState(true);
    }
    
    public void DisableKillZone()
    {
        killZone.SetState(false);
    }
    
    
}
