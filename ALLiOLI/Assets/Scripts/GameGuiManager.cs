using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameGuiManager : NetworkBehaviour
{

    [Space] [SerializeField] private GameObject clientsPanel;

    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenu;
    
    [Header("Lobby")]
    [SerializeField] private GameObject clientVisualizationPrefab;
    [SerializeField] private GameObject startMatchButton;
    [SerializeField] private GameObject lobby;
    
    
#pragma warning disable 414
    [SyncVar (hook = nameof(SetActive))] private bool lobbyVisible = true; // Default value is useful, bc the hook will only be called if the value in the var changes
#pragma warning restore 414
    private void SetActive(bool oldVal, bool newVal) { lobby.SetActive(newVal); }

    public void SetupOnlineLobby()
    {
        startMatchButton.SetActive(isServer);

        clientsPanel.transform.DestroyAllChildren();

        List<Client> clients = MatchManager.instance.Clients;

        if (clients != null)
            foreach (Client client in clients)
            {
                GameObject go = Instantiate(clientVisualizationPrefab, transform.position + new Vector3(0, 1, 0),
                    Quaternion.identity);
                go.transform.SetParent(clientsPanel.transform, false);
                go.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 0.7f, 0.9f, 1f, 1f);
            }
    }

    [Server]
    public void StartMatch()
    {
        lobbyVisible = false;
        MatchManager.instance.StartMatch();
    }

    public void ShowPauseMenu(bool show)
    {
        this.pauseMenu.SetActive(show);
    }
}