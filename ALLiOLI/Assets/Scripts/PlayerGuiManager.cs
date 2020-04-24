using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGuiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text trapsCounter;
    [SerializeField] private GameObject frame;

    public void SetCurrentNumberOfTraps(int currentTraps, int maxNumberOfTraps)
    {
        trapsCounter.text = currentTraps+"/"+maxNumberOfTraps;
    }

    public void SetColor(Color color)
    {
        foreach (Image frameSide in frame.GetComponentsInChildren<Image>())
        {
            frameSide.color = color;
        }
    }
}
