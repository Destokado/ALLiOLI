using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumTrap : Trap
{
    [SerializeField] private HingeJoint ball;

    protected override void Reload()
    {
        JointSpring hingeSpring = ball.spring;
        hingeSpring.spring = 2500;
        hingeSpring.damper = 0;
        hingeSpring.targetPosition = 85;
        ball.spring = hingeSpring;
        ball.useSpring = true;
    }

    public override void Activate()
    {
        base.Activate();
        
        ball.useSpring = false;
    }
    
}
