using Mirror;
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
        MatchPhase curPhase = MatchManager.Instance.CurrentPhase;
        matchTimerGameObject.SetActive(curPhase != null && curPhase.showMatchTimer);
        matchInformativeText.SetText(curPhase != null ? curPhase.informativeText : "");
    }

    public void UpdateEndScreen(bool forceSetActive = false)
    {
        if (forceSetActive)
            endScreen.SetActive(true);
        
        Player winner = (NetworkManager.singleton as AllIOliNetworkManager)?.GetPlayer(MatchManager.Instance.WinnerPlayerNetId);
        string winnerName = winner != null ? winner.gameObject.name : "NULL";
        endScreenText.SetText("The winner is" + winnerName + "!");
    }
}