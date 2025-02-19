﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkAnimator))]
public class AnimationTrap : Trap
{
    private NetworkAnimator trapAnimator;

    protected override void Awake()
    {
        base.Awake();
        trapAnimator = gameObject.GetComponentRequired<NetworkAnimator>();
    }

    public override void Activate()
    {
        base.Activate();
        trapAnimator.animator.SetTrigger("Activated");
    }
}
