using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsConfiguration
{
    public static ControlsConfiguration Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ControlsConfiguration();
            return _instance;
        }
    }
    private static ControlsConfiguration _instance;
    
    public float sensitivity = 1;
    public bool splitScreen = false;
    
}
