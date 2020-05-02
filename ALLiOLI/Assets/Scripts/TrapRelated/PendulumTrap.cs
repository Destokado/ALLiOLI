using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class PendulumTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private Rigidbody ball;

    public void Awake() 
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        ((TransformAnimation)animManager.GetAnimation(0)).originTransform.SetProperties(ball.transform);
        ball.isKinematic = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        
        animManager.Stop(0);
        ball.isKinematic = false;
    }
    
}
