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
        ShowNumberOfTraps(player.ownedTraps.Count, player.maxOwnableTraps);
        ShowReadiness(player.isReady);
    }

    public void ShowNumberOfTraps(int currentTraps, int maxNumberOfTraps)
    {
        trapsCounterGameobject.SetActive(MatchManager.Instance.currentPhase.showTrapsCounter);
        trapsCounter.SetText(currentTraps + "/" + maxNumberOfTraps);
    }


    public void ShowReadiness(bool isReady)
    {
        readinessGameObject.SetActive(MatchManager.Instance.currentPhase.showReadiness);
        readinessText.SetText(isReady ? "READY" : "Waiting");
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