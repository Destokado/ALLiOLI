using UnityEngine;

public class SlapTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;
    [SerializeField] private float startAngle;
    [SerializeField] private float targetAngle;

    protected override void Reload()
    {
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = startAngle;
        joint.spring = jointSpring;
    }

    public override void RpcActivate()
    {
        base.RpcActivate();
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = targetAngle;
        joint.spring = jointSpring;
    }
}