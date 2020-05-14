using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PoyMultiplayerSpawner : NetworkBehaviour
{
    
    [SerializeField] private GameObject otherPrefab;

    private void Start()
    {
        CmdSpawn();
    }

    [Command]
    void CmdSpawn()
    {
        GameObject go = Instantiate(otherPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity);
        NetworkServer.Spawn(go, connectionToClient);
    }
    
}
