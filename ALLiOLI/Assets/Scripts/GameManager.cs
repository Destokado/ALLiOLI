using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public List<Client> clients = new List<Client>();

    public int totalPlayers
    {
        get
        {
            int total = 0;
            foreach (Client client in singleton.clients)
                total += client.playerManager.players.Count;
            return total;
        }
    }

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