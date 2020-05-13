using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchGuiManager : MonoBehaviour
{

    [SerializeField] private GameObject matchTimerGameObject;
    [SerializeField] private TMP_Text matchTimer;
    [Space]
    [SerializeField] private TMP_Text matchInformativeText;
    [Space]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endScreenText;

    public void SetTimer(float timeInSeconds)
    {
        int seconds = (int) (timeInSeconds % 60);
        int minutes = (int) (timeInSeconds / 60);
        string time = minutes + ":" + seconds;

        matchTimer.text = time;
    }
    
    public void SetupForCurrentPhase()
    {
        matchTimerGameObject.SetActive(MatchManager.Instance.currentPhase.showMatchTimer);
        matchInformativeText.SetText(MatchManager.Instance.currentPhase.informativeText);
    }

    public void ShowEndScreen(string winner)
    {
        endScreen.SetActive(true);
        endScreenText.SetText("The winner is" + winner + "!");
    }
}
