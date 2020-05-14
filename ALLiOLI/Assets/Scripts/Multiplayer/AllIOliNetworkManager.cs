using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AllIOliNetworkManager : NetworkManager
{
    private List<Client> clients = new List<Client>(); // Only known by server (rn)

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // Same as base method
        Transform startPos = GetStartPosition();
        GameObject clientGo = startPos != null ? Instantiate(playerPrefab, startPos.position, startPos.rotation) : Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, clientGo);
        
        // Easy track of clients
        Client client = clientGo.GetComponent<Client>();
        clients.Add(client);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
        
        //TODO: Check if it is necessary to remove the client from the clients list (maybe the value becomes null or at least the clients[n].gameObject is null
    }
    
}
