using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : List<Trap>
{
    public Trap GetBestTrapToActivate()
    {
        SortedList<float, Trap> trapsByDistance = new SortedList<float, Trap>();

        foreach (Trap trap in this)
        {
            if (!trap.ItActivatable()) continue;
            
            float distance = float.MaxValue;
            foreach (Player player in MatchManager.Instance.players)
            {
                float trapDistance = trap.GetDistanceTo(player.character);
                if (trapDistance < distance)
                    distance = trapDistance;
            }
            trapsByDistance.Add(distance, trap);
        }

        if (trapsByDistance.Count > 0)
            return null;
        return trapsByDistance.Values[0];
    }
}
