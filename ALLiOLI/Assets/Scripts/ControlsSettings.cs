using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettings : MonoBehaviour
{
    [SerializeField] private Toggle splitScreenToggle;
    
    public void NewSensitivity (float value)
    {
        GlobalConfiguration.Instance.sensitivity = value;
    }
    
    public void NewSplitScreen (bool value)
    {
        GlobalConfiguration.Instance.splitScreen = value;
        splitScreenToggle.interactable  = !value; // It can only be set to "on" from "off"
    }

}
