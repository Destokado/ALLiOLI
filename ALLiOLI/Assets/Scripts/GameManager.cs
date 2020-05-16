using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public List<Client> clients = new List<Client>();
    
    public int totalPlayers => singleton.clients.Sum(client => client.PlayersManager.players.Count);

    [SerializeField] public Color[] playerColors;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }

        singleton = this;
    }
}