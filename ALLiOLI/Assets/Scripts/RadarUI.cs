using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    private List<Image> radarIndicator = new List<Image>();
    [SerializeField] private GameObject playerInRadarIndicator;

    public void RenderReport(List<KeyValuePair<Player, float>> report)
    {
        int dif = report.Count - radarIndicator.Count;
        
        if (dif > 0) // missing
        {
            for (int i = 0; i < dif; i++)
                radarIndicator.Add(Instantiate(playerInRadarIndicator, this.transform).GetComponent<Image>());
        }

        for (int r = 0; r < report.Count; r++)
        {
            KeyValuePair<Player, float> rep = report[r];
            radarIndicator[r].color = rep.Key.color;
            radarIndicator[r].rectTransform.anchorMax = new Vector2(rep.Value, radarIndicator[r].rectTransform.anchorMax.y);
            radarIndicator[r].rectTransform.anchorMin = new Vector2(rep.Value, radarIndicator[r].rectTransform.anchorMin.y);
            radarIndicator[r].gameObject.SetActive(true);
        }

        if (dif < 0)
        {
            for (int i = 0; i < -dif; i++)
                radarIndicator[radarIndicator.Count-dif].gameObject.SetActive(false);
        }
        
    }
}
