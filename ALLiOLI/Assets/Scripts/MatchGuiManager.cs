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
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endScreenText;

    private void Awake()
    {
        endScreen.SetActive(false);
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
            curPhase = MatchManager.instance.CurrentPhase;
        matchTimerGameObject.SetActive(curPhase != null && curPhase.showMatchTimer);
        matchInformativeText.SetText(curPhase != null ? curPhase.informativeText : "");
    }

    public void UpdateEndScreen(bool forceSetActive = false)
    {
        if (forceSetActive)
            endScreen.SetActive(true);
        
        Player winner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(MatchManager.instance.WinnerPlayerNetId);
        string winnerName = winner != null ? winner.gameObject.name : "NULL";
        endScreenText.SetText("The winner is " + winnerName + "!");
    }

    public void GoToLandingScene()
    {
        GameManager.Instance.ExitGame();
    }

}