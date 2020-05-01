using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKillZoneAtVelocity : MonoBehaviour
{
    [SerializeField] private bool awakeDisabled = true;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private KillZone killZone;
    [SerializeField] private float velocityToDisable;

    private void Awake()
    {
        if (awakeDisabled)
            enabled = false;
    }

    private void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude <= velocityToDisable)
            killZone.SetState(false);
    }

    public void ResetKillZone()
    {
        killZone.SetState(true);
    }
}
