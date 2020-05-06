using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(PlayAnimation), 5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ON COLLISION ENTER ");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ON TRIGGER ENTER");
    }

    private void PlayAnimation()
    {
        GetComponent<SimpleAnimationsManager>().Play(0);
    }
}
