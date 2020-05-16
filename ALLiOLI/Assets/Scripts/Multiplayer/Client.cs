using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayersManager))]
[RequireComponent(typeof(PlayerInputManager))]
public class Client : NetworkBehaviour
{
    [SyncVar] public int clientId;
    public static Client localClient { get; private set; }
    public PlayersManager PlayersManager { get; private set; }

    // Called on the server (when this NetworkBehaviour is network-ready)
    public override void OnStartServer()
    {
        base.OnStartServer();
        clientId = connectionToClient.connectionId;
    }

    // Called on all clients (when this NetworkBehaviour is network-ready)
    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(NetworkManager.singleton.transform, false);
        GameManager.singleton.clients.Add(this);

        if (LobbyManager.singleton.gameObject.activeSelf)
            LobbyManager.singleton.SetupLobby();

        gameObject.name = "Client " + clientId;
        
        PlayersManager = GetComponent<PlayersManager>();
    }

    // Called on the local client (when this NetworkBehaviour is network-ready)
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