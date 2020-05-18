using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public List<Client> clients = new List<Client>();

    public static int TotalPlayers => singleton.clients.Sum(client => client.PlayersManager.players.Count);

    [SerializeField] public Color[] playerColors;
    public const int maxPlayerCount = 16;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }

        singleton = this;
    }

    public bool AreAllPlayersReady()
    {
        foreach (Client client in clients)
            foreach (Player player in client.PlayersManager.players)
                if (!player.isReady)
                    return false;

        return true;
    }
}