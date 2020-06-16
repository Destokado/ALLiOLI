using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSettings : MonoBehaviour
{
    public void NewSensitivity (float value)
    {
        ControlsConfiguration.Instance.sensitivity = value;
    }
    
    public void NewSplitScreen (bool value)
    {
        ControlsConfiguration.Instance.splitScreen = value;
    }
}
