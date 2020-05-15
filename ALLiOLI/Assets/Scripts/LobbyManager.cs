using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager singleton;

    [Space] [SerializeField] private GameObject clientsPanel;

    [SerializeField] private GameObject clientVisualizationPrefab;

    [SerializeField] private string matchScene;
    [SerializeField] private GameObject startMatchButton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Debug.LogWarning("Multiple LobbyManager exist", this);
    }

    public void SetupLobby()
    {
        startMatchButton.SetActive(isServer);

        clientsPanel.transform.DestroyAllChildren();

        List<Client> clients = GameManager.singleton.clients;

        if (clients != null)
            foreach (Client client in clients)
            {
                GameObject go = Instantiate(clientVisualizationPrefab, transform.position + new Vector3(0, 1, 0),
                    Quaternion.identity);
                go.transform.SetParent(clientsPanel.transform, false);
                go.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 0.9f, 0.9f, 1f, 1f);
            }
    }

    [Server] // Allows only the server to start a match 
    public void StartMatch()
    {
        MatchManager.Instance.SetNewMatchPhase(new WaitingForPlayers());
        gameObject.SetActive(false); // Hide the lobby
    }
    
}