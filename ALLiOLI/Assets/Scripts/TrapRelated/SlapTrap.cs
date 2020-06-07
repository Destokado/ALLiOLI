using UnityEngine;

public class SlapTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;
    [SerializeField] private float startAngle;
    [SerializeField] private float targetAngle;

    protected override void Reload()
    {
        base.Reload();
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = startAngle;
        joint.spring = jointSpring;
    }

    public override void Activate()
    {
        base.Activate();
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = targetAngle;
        joint.spring = jointSpring;
    }
}