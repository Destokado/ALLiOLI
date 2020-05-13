using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;
    [SerializeField] private float startAngle;
    [SerializeField] private float targetAngle;
    protected override void Reload()
    {
        var jointSpring = joint.spring;
        jointSpring.targetPosition = startAngle;
        joint.spring = jointSpring;
       
    }

    public override void Activate()
    {
        base.Activate();
        var jointSpring = joint.spring;
        jointSpring.targetPosition = targetAngle;
        joint.spring = jointSpring;
        
    }
}
