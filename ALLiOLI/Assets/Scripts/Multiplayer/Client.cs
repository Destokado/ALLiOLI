using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class Client : NetworkBehaviour
{
    [SyncVar] public int clientId;
    public static Client localClient { get; private set; }
    public PlayerManager playerManager { get; private set; }

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

        if (LobbyManager.singleton.gameObject.activeSelf)
            LobbyManager.singleton.SetupLobby();

        playerManager = GetComponent<PlayerManager>();
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