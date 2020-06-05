using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGuiManager : MonoBehaviour
{
    [Space] [SerializeField] private GameObject frame;

    [Space] [SerializeField] private RadarUI radarUi;

    [Space] [SerializeField] private GameObject readinessGameObject;

    [SerializeField] private TMP_Text readinessText;
    
    [Space] [SerializeField] private TMP_Text ammunition;

    public void SetColor(Color color)
    {
        foreach (Image frameSide in frame.GetComponentsInChildren<Image>()) frameSide.color = color;
    }

    public void SetupForCurrentPhase(Player player)
    {
        GameManager.Instance.UpdateCursorMode();
        ShowReadiness(player);
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

    public void SetAmmunitionTo(int value)
    {
        ammunition.SetText(value.ToString());
    }
}