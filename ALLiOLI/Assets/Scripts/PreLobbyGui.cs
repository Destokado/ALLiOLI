using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class PreLobbyGui : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipTextField;
    [SerializeField] private GameObject toConnectObjects;
    [SerializeField] private GameObject connectingObjects;
    [SerializeField] private TMP_Text connectingText;


    private void Update()
    {
        if (NetworkClient.isConnected || NetworkServer.active)
            return;
        
        bool connecting = NetworkClient.active;
        toConnectObjects.SetActive(!connecting);
        connectingObjects.SetActive(connecting);
        
    }

    public void Host()
    {
        Debug.Log("Starting to host a match");
        AllIOliNetworkManager.singleton.StartHost();
    }
    
    public void JoinAsClient()
    {
        string text = ipTextField.text;
        if (text.IsNullOrEmpty())
            text = AllIOliNetworkManager.singleton.networkAddress;
        
        
        AllIOliNetworkManager.singleton.networkAddress = text;
        connectingText.SetText($"Connecting...");
        Debug.Log($"Joining '{AllIOliNetworkManager.singleton.networkAddress}'");
        AllIOliNetworkManager.singleton.StartClient();
    }

    public void Exit()
    {
        CancelConnection();
        GameManager.Instance.ExitScene();
    }
    
    public void CancelConnection()
    {
        AllIOliNetworkManager.singleton.StopClient();
    }

    public void SetNewName(String newName)
    {
        GlobalConfiguration.Instance.clientName = newName;
    }
}
