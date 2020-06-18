using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalConfiguration
{
    public static GlobalConfiguration Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GlobalConfiguration();
            return _instance;
        }
    }
    private static GlobalConfiguration _instance;
    
    public float sensitivity = 1;

    public bool splitScreen
    {
        get => _splitScreen;
        set
        {
            if (value == _splitScreen)
                return;

            PlayerInputManager pim = Client.LocalClient.PlayersManager.playerInputManager;
            //FieldInfo prop = pim.GetType().GetField("m_MaxPlayerCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //if (prop != null)  prop.SetValue(pim, value ? 4 : 1);
            //else Debug.LogWarning("PROP IS NULL");
            if (!value) 
                pim.DisableJoining();
            else
                pim.EnableJoining();
            
            _splitScreen = value;
        }
    }

    private bool _splitScreen = false;
    public string clientName = "";
}
