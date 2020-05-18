using UnityEngine;

public class PendulumTrap : Trap
{
    [SerializeField] private HingeJoint ball;

    protected override void Reload()
    {
        JointSpring hingeSpring = ball.spring;
        hingeSpring.spring = 10000;
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