using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    [SerializeField] private GameObject playerInRadarIndicator;
    private readonly List<Image> radarIndicator = new List<Image>();

    public void RenderReport(List<KeyValuePair<Trap, SortedList<float, Character>>> report)
    {
        int charactersInAllTraps = report.Sum(r => r.Value.Count);
        int dif = charactersInAllTraps - radarIndicator.Count;

        if (dif > 0) // missing
            for (int i = 0; i < dif; i++)
                radarIndicator.Add(Instantiate(playerInRadarIndicator, transform).GetComponent<Image>());

        int radarIndicatorId = 0;
        for (int t = 0; t < report.Count; t++)
        for (int c = 0; c < report[t].Value.Count; c++)
        {
            KeyValuePair<Trap, SortedList<float, Character>> rep = report[t];
            float clampedValue = Mathf.Clamp(rep.Value.Keys[c], 0f, 1f);
            radarIndicator[radarIndicatorId].color = rep.Value.Values[c].Owner.Color;
            radarIndicator[radarIndicatorId].rectTransform.anchorMax =
                new Vector2(clampedValue, radarIndicator[t].rectTransform.anchorMax.y);
            radarIndicator[radarIndicatorId].rectTransform.anchorMin =
                new Vector2(clampedValue, radarIndicator[t].rectTransform.anchorMin.y);
            radarIndicator[radarIndicatorId].gameObject.SetActive(true);
            radarIndicatorId++;
        }

        if (dif < 0) // too much
            for (int i = -dif; i > 0; i--)
                radarIndicator[radarIndicator.Count - i].gameObject.SetActive(false);
    }
}