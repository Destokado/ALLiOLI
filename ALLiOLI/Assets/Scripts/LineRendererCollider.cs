using System;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class LineRendererCollider : MonoBehaviour
{
    private CapsuleCollider capsuleColider;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        capsuleColider = GetComponent<CapsuleCollider>();
        capsuleColider.radius = lineRenderer.startWidth / 2f;
        capsuleColider.center = Vector3.zero;
        capsuleColider.direction = 2; // Z-axis for easier "LookAt" orientation
    }

    private void Update()
    {
        Vector3 startPoint = lineRenderer.GetPosition(0);
        Vector3 endPoint = lineRenderer.GetPosition(1);

        capsuleColider.transform.position = startPoint + (endPoint - startPoint) / 2f;
        capsuleColider.transform.LookAt(startPoint);
        capsuleColider.height = (endPoint - startPoint).magnitude;
    }
    
}