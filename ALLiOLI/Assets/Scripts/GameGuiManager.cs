using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameGuiManager : MonoBehaviour
{
    private enum Menu
    {
        SETTINGS,
        CONTROLS
    }

    private Menu currentMenu;


    [Header("PreLobby")] [SerializeField] private GameObject preLobby;

    [Header("PauseMenu")] [SerializeField] private GameObject pauseMenu;

    [Header("SettingsMenu")] [SerializeField]
    private GameObject settingsMenu;

    [Header("ControlsMenu")] [SerializeField]
    private GameObject controlsMenu;

    private void Awake()
    {
        // Set start values
        preLobby.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void SetStartMatchConfiguration()
    {
        preLobby.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void UpdateOnlineLobby(bool showLobby)
    {
        /*if (Client.LocalClient == null)
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
        
        lobby.SetActive(showLobby);*/
    }

    public void Back()
    {
        switch (currentMenu)
        {
            case Menu.SETTINGS:
                ShowSettingsMenu(false);
                break;
            case Menu.CONTROLS:
                ShowControlsMenu(false);
                break;
        }

        ShowPauseMenu(true);
    }

    public void StartMatch()
    {
        MatchManager.instance.StartMatch();
    }

    public void ShowPauseMenu(bool show)
    {
        this.pauseMenu.SetActive(show);
    }

    public void Resume()
    {
        GameManager.Instance.SetPause(false);
    }

    public void ShowControlsMenu(bool show)
    {
        controlsMenu.SetActive(show);
        currentMenu = Menu.CONTROLS;
    }

    public void Quit()
    {
        GameManager.Instance.QuitClient();
    }

    public void ExitScene()
    {
        GameManager.Instance.ExitScene();
    }

    public void ShowSettingsMenu(bool show)
    {
        settingsMenu.SetActive(show);
        currentMenu = Menu.SETTINGS;
    }
}