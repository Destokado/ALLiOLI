using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreLobbyGui : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipTextField;

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
        
        Debug.Log($"Joining '{text}'");
        AllIOliNetworkManager.singleton.networkAddress = text;
        AllIOliNetworkManager.singleton.StartClient();
    }
}
