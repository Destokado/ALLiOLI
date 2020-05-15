using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Client : NetworkBehaviour
{
    
    [SyncVar] public int clientId;
    public HashSet<Player> players { get; private set; }
    public static Client localClient { get; private set; }

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
        players = new HashSet<Player>();
    }
    
    // Called on the local client (when this player object is network-ready)
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localClient = this;
    }
    
    //Called on remaining clients, when a client disconnects
    public override void OnStopClient()
    {
        base.OnStopClient();
        GameManager.singleton.clients.Remove(this);
    }

}
