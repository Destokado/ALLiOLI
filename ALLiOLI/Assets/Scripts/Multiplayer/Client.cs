using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Client : NetworkBehaviour
{
    [SyncVar] public int clientId;

    // Called on the server (when this player object is network-ready)
    public override void OnStartServer()
    {
        base.OnStartServer();
        clientId = connectionToClient.connectionId;
    }
    
    // Called on all clients (when this player object is network-ready)
    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.SetParent(NetworkManager.singleton.transform, false);
        GameManager.singleton.clients.Add(this);
        LobbyManager.singleton.SetupLobby();
    }
    
    // Called on the local client (when this player object is network-ready)
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }
    
    //Called on remaining clients, when a client disconnects
    public override void OnStopClient()
    {
        base.OnStopClient();
        GameManager.singleton.clients.Remove(this);
    }

}
