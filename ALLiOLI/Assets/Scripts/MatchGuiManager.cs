using TMPro;
using UnityEngine;

public class MatchGuiManager : MonoBehaviour
{
    [Space] [SerializeField] private GameObject endScreen;

    [SerializeField] private TMP_Text endScreenText;

    [Space] [SerializeField] private TMP_Text matchInformativeText;

    [SerializeField] private TMP_Text matchTimer;

    [SerializeField] private GameObject matchTimerGameObject;

    public void SetTimer(float timeInSeconds)
    {
        int seconds = (int) (timeInSeconds % 60);
        int minutes = (int) (timeInSeconds / 60);
        string time = minutes + ":" + seconds;

        matchTimer.text = time;
    }

    public void SetupForCurrentPhase()
    {
        MatchPhase curPhase = MatchManager.Instance.currentPhase;
        matchTimerGameObject.SetActive(curPhase != null && curPhase.showMatchTimer);
        matchInformativeText.SetText(curPhase != null ? curPhase.informativeText : "");
    }

    public void ShowEndScreen(string winner)
    {
        endScreen.SetActive(true);
        endScreenText.SetText("The winner is" + winner + "!");
    }
}