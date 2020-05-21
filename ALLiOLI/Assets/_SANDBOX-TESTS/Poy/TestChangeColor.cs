using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChangeColor : MonoBehaviour
{
    private static readonly int baseColor = Shader.PropertyToID("_BaseColor");

    void Start()
    {
        // You can re-use this block between calls rather than constructing a new one each time.
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        // You can look up the property by ID instead of the string to be more efficient.
        block.SetColor(baseColor, Color.red);
        // You can cache a reference to the renderer to avoid searching for it.
        GetComponent<MeshRenderer>().SetPropertyBlock(block);
    }

}
