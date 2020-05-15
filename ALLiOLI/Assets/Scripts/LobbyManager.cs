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
    [SyncVar (hook = nameof(SetActive))] private bool isVisible = true; // Default value is useful, bc the hook will only be called if the value in the var changes
    private void SetActive(bool oldVal, bool newVal) { gameObject.SetActive(newVal); }

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

    [Server]
    public void StartMatch()
    {
        isVisible = false;
        Debug.Log("SERVER: isVisible? " + isVisible);
        MatchManager.Instance.BroadcastNewMatchPhase(new WaitingForPlayers());
    }
    
}