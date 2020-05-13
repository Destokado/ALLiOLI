
using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class SwordsTrap : SlapTrap
{
    [SerializeField] private HingeJoint secondJoint;
    [SerializeField] private float secondSstartAngle;
    [SerializeField] private float secondTargetAngle;
    //[SerializeField] private KillZone secondKillZone;
  

    protected override void Reload()
    {
         base.Reload();
         var jointSpring = secondJoint.spring;
         jointSpring.targetPosition = secondSstartAngle;
         secondJoint.spring = jointSpring;
         //secondKillZone.enabled = false;
    }

    public override void Activate()
    {
         base.Activate();
        var jointSpring = secondJoint.spring;
        jointSpring.targetPosition = secondTargetAngle;
        secondJoint.spring = jointSpring;
        //secondKillZone.enabled = true;
    }
    
}

