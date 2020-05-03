using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;

  

    // Update is called once per frame
   

    protected override void Reload()
    {
        base.Activate();
        var jointSpring = joint.spring;
        jointSpring.targetPosition = 0;
        joint.spring = jointSpring;
    }

    public override void Activate()
    {
        base.Activate();

        var spring = joint.spring;
        spring.targetPosition = 179;
        joint.spring = spring;
    }
}
