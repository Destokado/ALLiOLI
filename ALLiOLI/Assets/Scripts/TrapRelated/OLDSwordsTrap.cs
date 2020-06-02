using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class OLDSwordsTrap : SlapTrap
{
    [SerializeField] private HingeJoint secondJoint;
    [SerializeField] private float secondSstartAngle;

    [SerializeField] private float secondTargetAngle;
    //[SerializeField] private KillZone secondKillZone;


    protected override void Reload()
    {
        base.Reload();
        JointSpring jointSpring = secondJoint.spring;
        jointSpring.targetPosition = secondSstartAngle;
        secondJoint.spring = jointSpring;
        //secondKillZone.enabled = false;
    }

    public override void Activate()
    {
        base.Activate();
        JointSpring jointSpring = secondJoint.spring;
        jointSpring.targetPosition = secondTargetAngle;
        secondJoint.spring = jointSpring;
        //secondKillZone.enabled = true;
    }
}