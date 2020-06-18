using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSettings : MonoBehaviour
{
    public void NewSensitivity (float value)
    {
        GlobalConfiguration.Instance.sensitivity = value;
    }
    
    public void NewSplitScreen (bool value)
    {
        GlobalConfiguration.Instance.splitScreen = value;
    }
}
