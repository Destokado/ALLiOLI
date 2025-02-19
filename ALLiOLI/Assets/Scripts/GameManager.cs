﻿using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public GameGuiManager GUI;
    
    public bool PauseMenuShowing
    {
        get => _pauseMenuShowing;
        private set
        {
            if (_pauseMenuShowing == value)
                return;
            
            _pauseMenuShowing = value;
            GUI.ShowPauseMenu(_pauseMenuShowing);
            UpdateCursorMode();
        }
    }
    // ReSharper disable once InconsistentNaming
    private bool _pauseMenuShowing = false;
    public bool escapeOnEditor;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created!", this);
            return;
        }

        Instance = this;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        UpdateCursorMode();
        escapeOnEditor = false;
    }

    public void UpdateCursorMode()
    {
        bool matchInGamingMode = MatchManager.instance != null && MatchManager.instance.currentPhase != null && MatchManager.instance.currentPhase.inGamingMode;
        //bool gamingMode = MatchManager.Instance.IsMatchRunning && !PauseMenuShowing && Application.isFocused;

        bool cursorGameMode = matchInGamingMode && Client.IsThereALocalPlayer() && !PauseMenuShowing && Application.isFocused;
        //Debug.Log($"Is cursor in in-game mode? {cursorGameMode}");
        
        SetCursorMode(cursorGameMode);
    }

    /// <summary>
    /// Sets the cursor to in-game or not in-game mode
    /// </summary>
    /// <param name="inGameMode">If true, the cursor is not visible and locked at the center of the screen. If false, the opposite.</param>
    public static void SetCursorMode(bool inGameMode)
    {
        Debug.Log($"Setting cursor in {inGameMode} gaming mode.");
        Cursor.visible = !inGameMode;
        Cursor.lockState = CursorLockMode.None; // Maybe bug fixing: http://answers.unity.com/answers/1119750/view.html
        Cursor.lockState = inGameMode ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void QuitClient()
    {
        if (Client.LocalClient == null)
        {
            Debug.Log("Exiting the game (not the scene) before the reference to the localClient is set. Expected if the initial connection to a server failed.");
            return;
        }
        
        Debug.Log($"Exiting the game. User exiting is client? {Client.LocalClient.isClient} - User exiting is server? {Client.LocalClient.isServer}");
        
        if (Client.LocalClient.isClient && Client.LocalClient.isServer)
        {
            Debug.Log("STOPPING HOST");   
            NetworkManager.singleton.StopHost();
        }
        
        if (Client.LocalClient.isClient)
        {
            Debug.Log("STOPPING CLIENT");            
            NetworkManager.singleton.StopClient();
        }
        
        if (Client.LocalClient.isServer)
        {
            Debug.Log("STOPPING SERVER");
            NetworkManager.singleton.StopServer();
        }

        ExitScene();
    }

    public void ExitScene()
    {
        string sceneName = (NetworkManager.singleton as AllIOliNetworkManager)?.nameOfDisconnectionFromServerScene;
        Debug.Log($"Loading scene {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void PauseButtonPressed()
    {
        PauseMenuShowing = !PauseMenuShowing;
        if(!PauseMenuShowing){}
    }
    
    public void SetPause(bool val)
    {
        PauseMenuShowing = val;
    }
}