using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public List<Client> clients = new List<Client>();
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

    public void SetAllPlayersAsNotReady()
    {
        foreach (Client client in clients)
            foreach (Player player in players)
                player.isReady = false;
    }
}