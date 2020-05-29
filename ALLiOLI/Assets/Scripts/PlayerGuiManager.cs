using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGuiManager : MonoBehaviour
{
    [Space] [SerializeField] private GameObject frame;

    [Space] [SerializeField] private GameObject interactionText;

    [Space] [SerializeField] private RadarUI radarUi;

    [Space] [SerializeField] private GameObject readinessGameObject;

    [SerializeField] private TMP_Text readinessText;
    [SerializeField] private TMP_Text trapsCounter;
    [SerializeField] private GameObject trapsCounterGameobject;

    public void SetColor(Color color)
    {
        foreach (Image frameSide in frame.GetComponentsInChildren<Image>()) frameSide.color = color;
    }

    public void SetupForCurrentPhase(Player player)
    {
        GameManager.Instance.UpdateCursorMode();
        ShowNumberOfTraps(player);
        ShowReadiness(player);
    }

    public void ShowNumberOfTraps(Player player)
    {
        bool show = MatchManager.instance.currentPhase.showTrapsCounter;
        trapsCounterGameobject.SetActive(show);
        
        if (!show)
            return;
        
        int currentTraps = player.HumanLocalPlayer.OwnedTraps.Count;
        int maxNumberOfTraps = player.HumanLocalPlayer.MaxOwnableTraps;
        trapsCounter.SetText(currentTraps + "/" + maxNumberOfTraps);
    }


    public void ShowReadiness(Player player)
    {
        bool show = MatchManager.instance.currentPhase.showReadiness;
        readinessGameObject.SetActive(show);
        
        if (!show)
            return;

        readinessText.SetText(player.isReady ? "READY" : "Waiting");
    }


    public void ReportInRadar(List<KeyValuePair<Trap, SortedList<float, Character>>> report)
    {
        radarUi.RenderReport(report);
    }

    public void ShowInteractionText(bool show)
    {
        interactionText.SetActive(show);
    }
}