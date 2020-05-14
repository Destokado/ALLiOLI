using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Client : NetworkBehaviour
{
    #region Initialization

    // These are set in OnStartServer and used in OnStartClient
    [SyncVar]
    int clientNo;
    
    // This fires on server when this player object is network-ready
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Set SyncVar values
        clientNo = connectionToClient.connectionId;

        // Start generating data updates
        InvokeRepeating(nameof(UpdateData), 1, 1);
    }
    
    // This fires on all clients when this player object is network-ready
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        // Make this a child of the layout panel in the Canvas
        transform.SetParent(NetworkManager.singleton.transform, false);
    }

    // This only fires on the local client when this player object is network-ready
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //CmdSetReady(); // Can be used here because is client

        LobbyManager.singleton.SetupLobby();
    }

    #endregion

    // Data changed by a client (on NetworkIdentities on which it has authority)
    #region PlayerIsReady
    
    [SyncVar(hook = nameof(OnReadyChanged))]
    private bool ready;
    
    public void OnReadyChanged(bool oldValue, bool newValue)
    {
        // Do something... Display it?...
    }
    
    [Command]
    public void CmdSetReady()
    {
        ready = true;
    }

    #endregion
    
    // Data changed by the server
    #region Data Example

    // This is updated by UpdateData which is called from OnStartServer via InvokeRepeating
    [SyncVar(hook = nameof(OnPlayerDataChanged))]
    public int playerData;
    
    // This is called by the hook of playerData SyncVar above
    void OnPlayerDataChanged(int oldPlayerData, int newPlayerData)
    {
        // Do something... Display it?...
    }

    // This only runs on the server, called from OnStartServer (at initialization) via InvokeRepeating
    [ServerCallback]
    void UpdateData()
    {
        playerData = Random.Range(100, 1000);
    }
    
    #endregion
    
    
}
