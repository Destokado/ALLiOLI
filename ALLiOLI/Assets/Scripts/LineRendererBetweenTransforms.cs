using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class LineRendererBetweenTransforms : MonoBehaviour
{
    [SerializeField] public List<Transform> points;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        if (points != null)
            for (int t = 0; t < points.Count; t++)
                lineRenderer.SetPosition(t, points[t].position);
    }
}
