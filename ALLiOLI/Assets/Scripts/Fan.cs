using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float maxEffectDistance = 3f;
    [SerializeField] private float force = 10f;
    [SerializeField] private AnimationCurve forceByDistance;

    private void OnTriggerStay(Collider other)
    {
        float distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance > maxEffectDistance)
            return;
        float forcePerByDistance = forceByDistance.Evaluate(distance / maxEffectDistance);
        Vector3 forceApplied = transform.forward * (force * forcePerByDistance);
        other.attachedRigidbody.AddForce(forceApplied, ForceMode.Impulse);
        
        Debug.DrawLine(other.transform.position, other.transform.position+forceApplied, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, maxEffectDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(position, position+transform.forward*force);
    }
}
