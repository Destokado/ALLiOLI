using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class LineRendererBetweenTransforms : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] public List<Transform> points;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (points != null)
            for (int t = 0; t < points.Count; t++)
                lineRenderer.SetPosition(t, points[t].position);
    }
}