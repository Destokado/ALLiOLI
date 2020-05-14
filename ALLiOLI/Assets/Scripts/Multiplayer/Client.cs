using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Client : NetworkBehaviour
{

    [Header("Child Text Objects")]
    public Text playerNameText;
    public Text playerDataText;

    // These are set in OnStartServer and used in OnStartClient
    [SyncVar]
    int clientNo;

    // This is updated by UpdateData which is called from OnStartServer via InvokeRepeating
    [SyncVar(hook = nameof(OnPlayerDataChanged))]
    public int playerData;
    public bool ready = false;

    // This is called by the hook of playerData SyncVar above
    void OnPlayerDataChanged(int oldPlayerData, int newPlayerData)
    {
        // Show the data in the UI
        playerDataText.text = string.Format("Data: {0:000}", newPlayerData);
    }

    // This fires on server when this player object is network-ready
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Set SyncVar values
        clientNo = connectionToClient.connectionId;

        // Start generating updates
        InvokeRepeating(nameof(UpdateData), 1, 1);
    }

    // This only runs on the server, called from OnStartServer via InvokeRepeating
    [ServerCallback]
    void UpdateData()
    {
        playerData = Random.Range(100, 1000);
    }

    // This fires on all clients when this player object is network-ready
    public override void OnStartClient()
    {
        base.OnStartClient();

        // Make this a child of the layout panel in the Canvas
        transform.SetParent(GameObject.Find("PlayersPanel").transform, false);

        // Apply SyncVar values
        playerNameText.text = string.Format("Client {0:00}", clientNo);
    }

    // This only fires on the local client when this player object is network-ready
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        ready = true;
        playerNameText.text += ". Ready? " + ready;
        
        Debug.Log("OnStartLocalPlayer");
    }
}
