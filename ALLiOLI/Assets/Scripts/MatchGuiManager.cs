using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchGuiManager : MonoBehaviour
{

    [SerializeField] private TMP_Text matchTimer;

    public void SetTimer(float timeInSeconds)
    {
        int seconds = (int) (timeInSeconds % 60);
        int minutes = (int) (timeInSeconds / 60);
        string time = minutes + ":" + seconds;

        matchTimer.text = time;
    }
}
