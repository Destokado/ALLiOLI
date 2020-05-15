using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class LineRendererCollider : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.radius = lineRenderer.startWidth / 2f;
    }

    private void Update()
    {
        float distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        capsuleCollider.height = distance;

        Vector3 capsuleColliderCenter = capsuleCollider.center;
        capsuleColliderCenter.y = distance / 2f;
        capsuleCollider.center = capsuleColliderCenter;

        //capsuleCollider.transform.rotation = Quaternion.LookRotation(lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0));
    }
}