using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float maxEffectDistance = 3f;
    [SerializeField] private float force = 50f;
    [SerializeField] private AnimationCurve forceByDistance;

    private void OnTriggerStay(Collider other)
    {
        float distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance > maxEffectDistance)
            return;
        float forcePerByDistance = forceByDistance.Evaluate(1 - (distance / maxEffectDistance));
        other.attachedRigidbody.AddForce(transform.forward * (force * forcePerByDistance), ForceMode.Impulse);
        Debug.Log($"ADDING FORCE TO {other.gameObject}");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, maxEffectDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position+transform.forward*force);
    }
}
