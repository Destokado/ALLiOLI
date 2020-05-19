using Mirror;
using UnityEngine;

public class AllIOliNetworkManager : NetworkManager
{
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
    
    public Player GetPlayer(uint playerNetId)
    {
        if (NetworkIdentity.spawned.ContainsKey(playerNetId))
            return NetworkIdentity.spawned[playerNetId].gameObject.GetComponent<Player>();
        
        Debug.LogWarning("Player with NetId " + playerNetId + " not found.");
        return null;
    }
}