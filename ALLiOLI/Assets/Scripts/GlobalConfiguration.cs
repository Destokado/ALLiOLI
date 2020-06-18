using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool splitScreen = false;
    public string clientName = "";
}
