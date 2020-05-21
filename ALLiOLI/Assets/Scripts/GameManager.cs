using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public List<Client> clients = new List<Client>();

    public static int TotalCurrentPlayers => singleton.clients.Sum(client => client.PlayersManager.players.Count);
    public static int indexOfLastPlayer = -1;

    [SerializeField] private Color[] playerColors;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }

        singleton = this;
    }

    public bool AreAllPlayersReady()
    {
        foreach (Client client in clients)
            foreach (Player player in client.PlayersManager.players)
                if (!player.isReady)
                    return false;

        return true;
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

    public Color GetColor(int playerIndex)
    {
        if (playerIndex < playerColors.Length)
            return playerColors[playerIndex];
        
        EasyRandom rnd = new EasyRandom(playerIndex);

        float hueMin = 0f;
        float hueMax = 1f;

        float saturationMin = 0.7f;
        float saturationMax = 0.9f;

        float valueMin = 1f;
        float valueMax = 1f;
        
        Color rgb = Color.HSVToRGB(Mathf.Lerp(hueMin, hueMax, rnd.GetRandomFloat()), Mathf.Lerp(saturationMin, saturationMax, rnd.GetRandomFloat()), Mathf.Lerp(valueMin, valueMax, rnd.GetRandomFloat()), true);
        rgb.a = 1f;

        //Random.ColorHSV(0, 1f, 0.7f, 0.9f, 1f, 1f);
        return rgb;
    }
}