using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Spawners have been created. Destroying the script of " + gameObject.name, gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject Spawn(GameObject prefab)
    {
        //TODO: Maybe it should be a pool?
        
        return Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
