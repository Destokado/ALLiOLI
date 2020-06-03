using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrap : Trap
{
    [SerializeField] private Animation trapAnimation;

    public override void Activate()
    {
        base.Activate();
        trapAnimation.Play();
    }
}
