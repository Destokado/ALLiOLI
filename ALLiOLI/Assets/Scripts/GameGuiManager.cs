using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameGuiManager : MonoBehaviour
{

    [Space] [SerializeField] private GameObject clientsPanel;

    [Header("PreLobby")]
    [SerializeField] private GameObject preLobby;
    
    [Header("Lobby")]
    [SerializeField] private GameObject clientVisualizationPrefab;
    [SerializeField] private GameObject startMatchButton;
    [SerializeField] private GameObject lobby;
    
    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenu;
    
    private void Awake()
    {
        // Set start values
        preLobby.SetActive(false);
        lobby.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void SetStartMatchConfiguration()
    {
        preLobby.SetActive(false);
        lobby.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void UpdateOnlineLobby(bool showLobby)
    {
        if (Client.LocalClient == null)
            return;

        startMatchButton.SetActive(Client.LocalClient.isServer);

        clientsPanel.transform.DestroyAllChildren();

        List<Client> clients = MatchManager.Instance.Clients;

        if (clients != null)
            foreach (Client client in clients)
            {
                GameObject go = Instantiate(clientVisualizationPrefab, transform.position + new Vector3(0, 1, 0),
                    Quaternion.identity);
                go.transform.SetParent(clientsPanel.transform, false);
                go.GetComponent<Image>().color = UnityEngine.Random.ColorHSV(0f, 1f, 0.7f, 0.9f, 1f, 1f);
            }
        
        lobby.SetActive(showLobby);
    }
    
    public void StartMatch()
    {
        lobby.SetActive(false);
        MatchManager.Instance.StartMatch();
    }

    public void ShowPauseMenu(bool show)
    {
        this.pauseMenu.SetActive(show);
    }

    public void Resume()
    {
        GameManager.Instance.SetPause(false);
    }
    
    public void ShowLobbyMenu(bool show)
    {
        //TODO: Implement lobby open/close
        Debug.LogWarning("lobby in pause not implemented yet");
    }
    
    public void ShowPlayersMenu(bool show)
    {
        //TODO: Implement players menu
        Debug.LogWarning("Players menu not implemented yet");
    }
    
    public void Quit()
    {
        GameManager.Instance.ExitGame();
    }
    
    public void ShowSettingsMenu(bool show)
    {
        //TODO: Implement settings
        Debug.LogWarning("Settings not implemented yet");
    }
}