using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }

        instance = this;
    }



    public void SetCursorMode(bool inGame)
    {
        Cursor.visible = !inGame;
        Cursor.lockState = inGame? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game. Client?" + Client.localClient.isClient + " - Server?" + Client.localClient.isServer);

        if (Client.localClient.isClient && Client.localClient.isServer)
        {
            Debug.Log("STOPPING HOST");   
            NetworkManager.singleton.StopHost();
        }
        
        if (Client.localClient.isClient)
        {
            Debug.Log("STOPPING CLIENT");            
            NetworkManager.singleton.StopClient();
        }
        
        if (Client.localClient.isServer)
        {
            Debug.Log("STOPPING SERVER");
            NetworkManager.singleton.StopServer();
        }

        string sceneName = (NetworkManager.singleton as AllIOliNetworkManager)?.nameOfDisconnectionFromServerScene;
        Debug.Log("Loading scene " + sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    
}