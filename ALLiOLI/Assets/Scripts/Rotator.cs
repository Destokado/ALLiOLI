using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float standardRotation;

    [SerializeField] private float maximumRotation;
    private float _deltaRotation;
    private bool isNewDeltaRotation;
    
    public float deltaRotation
    {
        get => _deltaRotation;
        set
        {
            _deltaRotation = value;
            isNewDeltaRotation = Math.Abs(value) > 0.001;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Mathf.Lerp(standardRotation,maximumRotation,deltaRotation) * Time.deltaTime, 0); 
        isNewDeltaRotation = false;
    }

    private void FixedUpdate()
    {
        if (!isNewDeltaRotation)
        {
            deltaRotation = 0;
        }
    }
}