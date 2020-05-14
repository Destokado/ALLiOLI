using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager singleton;

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
    }
    
    [Server]
    public void StartMatch()
    {
        RpcStartMatchAllClients();
    }

    [ClientRpc]
    public void RpcStartMatchAllClients()
    {
        SceneManager.LoadScene(matchScene, LoadSceneMode.Single);
    }
}
