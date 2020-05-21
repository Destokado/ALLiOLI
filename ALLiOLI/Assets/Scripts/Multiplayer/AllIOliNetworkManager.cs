using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllIOliNetworkManager : NetworkManager
{
    
    [SerializeField] private Object disconnectionFromServerScene;
    public string nameOfDisconnectionFromServerScene => disconnectionFromServerScene.name;

    // When a player/client is connected
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        /* // Same as base method
        Transform startPos = GetStartPosition();
        GameObject clientGo = startPos != null ? Instantiate(playerPrefab, startPos.position, startPos.rotation) : Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, clientGo);
        
        // Easy track of clients
        Client client = clientGo.GetComponent<Client>();
        clients.Add(client);*/
    }

    // When a player/client is disconnected
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

    // Called on clients when disconnected form a server
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        
        GameManager.Instance.ExitGame();
    }

    // Be aware: can return null if "Player" component is not found or if the GameObject with given NetId is not found
    public Player GetPlayer(uint playerNetId)
    {
        if (NetworkIdentity.spawned.ContainsKey(playerNetId))
            return NetworkIdentity.spawned[playerNetId].gameObject.GetComponent<Player>(); 
        
        Debug.LogWarning("GameObject with NetId " + playerNetId + " not found.");
        return null;
    }
}