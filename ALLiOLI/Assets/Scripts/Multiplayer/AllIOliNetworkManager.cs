using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AllIOliNetworkManager : NetworkManager
{
    
    private List<Client> clients = new List<Client>();
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // Same as base method
        Transform startPos = GetStartPosition();
        GameObject client = startPos != null ? Instantiate(playerPrefab, startPos.position, startPos.rotation) : Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, client);
        
        // New code, needed the reference to the player
        clients.Add(client.GetComponent<Client>());
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
        
        //TODO: Check if it is necessary to remove the client from the clients list (maybe the value becomes null or at least the clients[n].gameObject is null
    }
    
}
