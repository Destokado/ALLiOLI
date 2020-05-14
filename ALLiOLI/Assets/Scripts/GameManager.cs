using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public List<Client> clients = new List<Client>();
    
    public static GameManager singleton;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }
        
        singleton = this;
    }
}
