using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    [SerializeField] private Image radarIndicator;
    [SerializeField] private Sprite[] imagesAnimation;

    public void RenderReport(List<KeyValuePair<Trap, SortedList<float, Character>>> report)
    {
        float distanceClosestChar = 1f; // [0, 1]
        Character closestCharacter = null;
        Trap closestTrap = null;
        foreach (KeyValuePair<Trap, SortedList<float, Character>> trapReport in report)
        {
            float distance = trapReport.Value.Keys[0];
            trapReport.Key.isHighlighted = false;

            if (distance < distanceClosestChar)
            {
                distanceClosestChar = distance;
                closestCharacter = trapReport.Value.Values[0];
                closestTrap = trapReport.Key;
            }
        }

        closestTrap.isHighlighted = true;

        int img = 0;
        if (closestCharacter != null)
        {
            img = Mathf.RoundToInt(distanceClosestChar * (imagesAnimation.Length - 1));
            radarIndicator.color = closestCharacter.Owner.Color;
            if (img == imagesAnimation.Length)
                SoundManager.Instance.PlayOneShotLocal(SoundManager.EventPaths.Alarm, Vector3.zero, null);
        }

        radarIndicator.sprite = imagesAnimation[img];
    }
}