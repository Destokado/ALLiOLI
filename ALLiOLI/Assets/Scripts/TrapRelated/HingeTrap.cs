using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class HingeTrap : Trap
{
    // Start is called before the first frame update
    [SerializeField] private HingeJoint joint;
    [SerializeField] private float startAngle;
    [SerializeField] private float targetAngle;
    [FormerlySerializedAs("reloadEmitter")] [SerializeField] private StudioEventEmitter resetEmitter;

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
        if (resetEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, resetEmitter.Event);
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