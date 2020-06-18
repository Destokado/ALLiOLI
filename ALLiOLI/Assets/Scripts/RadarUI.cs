using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class RadarUI : MonoBehaviour
{
    [SerializeField] private Image radarIndicator;
    [SerializeField] private Sprite[] imagesAnimation;

    private EventInstance alarmEvent;
    public void RenderReport(List<KeyValuePair<Trap, SortedList<float, Character>>> report)
    {
        float distanceClosestChar = 1f; // [0, 1]
        Character closestCharacter = null;
        Trap closestTrap = null;
        foreach (KeyValuePair<Trap, SortedList<float, Character>> trapReport in report)
        {
            float distance = trapReport.Value.Keys[0] / (!trapReport.Value.Values[0].HasFlag ? 1:1.7f);
            trapReport.Key.isHighlighted = false;

            if (distance < distanceClosestChar)
            {
                distanceClosestChar = distance;
                closestCharacter = trapReport.Value.Values[0];
                closestTrap = trapReport.Key;
            }
        }

        if (closestTrap != null && !closestTrap.OnCd)
            closestTrap.isHighlighted = true;

        int img = imagesAnimation.Length - 1;
        if (closestCharacter != null)
        {
            img = Mathf.RoundToInt(distanceClosestChar * (imagesAnimation.Length - 1));

            radarIndicator.color = closestCharacter.Owner.Color;
            if (img == 0)
            {
                if (!SoundManager.Instance.isPlaying(SoundManager.EventPaths.Alarm))
                   alarmEvent= SoundManager.Instance.PlayEventLocal(SoundManager.EventPaths.Alarm, Vector3.zero);
            }
        }

        radarIndicator.sprite = imagesAnimation[img];
    }
}