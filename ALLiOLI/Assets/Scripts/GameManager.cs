using System.Collections.Generic;
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
    }

    public void UpdateCursorMode()
    {
        bool matchInGamingMode = MatchManager.Instance != null && MatchManager.Instance.CurrentPhase != null && MatchManager.Instance.CurrentPhase.inGamingMode;
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
        Cursor.visible = !inGameMode;
        Cursor.lockState = inGameMode ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ExitGame()
    {
        Debug.Log($"Exiting the game. Client? {Client.LocalClient.isClient} - Server? {Client.LocalClient.isServer}");

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

        string sceneName = (NetworkManager.singleton as AllIOliNetworkManager)?.nameOfDisconnectionFromServerScene;
        Debug.Log($"Loading scene {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void PauseButtonPressed()
    {
        PauseMenuShowing = !PauseMenuShowing;
    }
    
    public void SetPause(bool val)
    {
        PauseMenuShowing = val;
    }
}