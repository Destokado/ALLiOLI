using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrap : Trap
{
    private SimpleAnimationsManager animManager;

    private void Awake()
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
    }
}
