using System;
using FMODUnity;
using UnityEngine;

public class HingeTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;
    [SerializeField] private float startAngle;
    [SerializeField] private float targetAngle;
    [SerializeField] private StudioEventEmitter reloadEmitter;

    private void Start()
    {
        if (isServer)
            SetDefaultState();
        
        Rigidbody rb = joint.GetComponentRequired<Rigidbody>();
        rb.centerOfMass = new Vector3(0,0,0);
        rb.inertiaTensor = new Vector3(1, 1, 1);
    }

    private void SetDefaultState()
    {
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = startAngle;
        joint.spring = jointSpring;
    }

    protected override void Reload()
    {
        base.Reload();
        if (reloadEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, reloadEmitter.Event);
        SetDefaultState();
    }

    public override void Activate()
    {
        base.Activate();
        JointSpring jointSpring = joint.spring;
        jointSpring.targetPosition = targetAngle;
        joint.spring = jointSpring;
    }
}