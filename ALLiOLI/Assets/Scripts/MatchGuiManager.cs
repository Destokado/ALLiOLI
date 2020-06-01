using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchGuiManager : MonoBehaviour
{
    [Header("In-game content")] [SerializeField] private TMP_Text matchInformativeText;
    [SerializeField] private TMP_Text matchTimer;
    [SerializeField] private GameObject matchTimerGameObject;

    [Header("End screen")] 
    [SerializeField] private GameObject endOfMatchScreen;
    [SerializeField] private TMP_Text endOfMatchScreenText;

    private void Awake()
    {
        endOfMatchScreen.SetActive(false);
        SetupForCurrentPhase();
    }

    public void SetTimer(float timeInSeconds)
    {
        int seconds = (int) (timeInSeconds % 60);
        int minutes = (int) (timeInSeconds / 60);
        string time = minutes + ":" + seconds;

        matchTimer.text = time;
    }

    public void SetupForCurrentPhase()
    {
        MatchPhase curPhase = null;
        if (MatchManager.instance != null)
            curPhase = MatchManager.instance.currentPhase;
        matchTimerGameObject.SetActive(curPhase != null && curPhase.showMatchTimer);
        UpdateInformativeText(curPhase != null ? curPhase.informativeText : "");
    }

    public void UpdateInformativeText(string text)
    {
        matchInformativeText.SetText(text);
    }

    public void UpdateEndScreen(bool forceSetActive = false)
    {
        if (forceSetActive)
            endOfMatchScreen.SetActive(true);
        
        endOfMatchScreenText.SetText($"The winner is {MatchManager.instance.roundWinnerName}!");
    }

    public void GoToLandingScene()
    {
        GameManager.Instance.QuitClient();
    }

}