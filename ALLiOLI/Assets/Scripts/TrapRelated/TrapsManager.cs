using System.Collections.Generic;
using System.Linq;

public class TrapsManager : List<Trap>
{
    /*public TrapManager(List<Trap> trapsInManager)
    {
        this.AddRange(trapsInManager);
    }*/

    public Trap GetBestTrapToActivate(Player playerToAvoid)
    {
        List<KeyValuePair<Trap, SortedList<float, Character>>>
            radarReport = GetCharactersInEachTrapRadar(playerToAvoid);
        if (radarReport.Count > 0)
            return radarReport[0].Key;

        SortedList<float, Trap> trapsByDistance = GetTrapsSortedByDistance();
        if (trapsByDistance.Count <= 0) return null;
        return trapsByDistance.Values[0];
    }

    public SortedList<float, Trap> GetTrapsSortedByDistance()
    {
        SortedList<float, Trap> trapsByDistance = new SortedList<float, Trap>();

        foreach (Trap trap in this)
        {
            if (!trap.IsActivatable()) continue;

            float distance = float.MaxValue;
            foreach (Client client in MatchManager.instance.clients)
                foreach (Player player in client.PlayersManager.players)
                {
                    float trapDistance = trap.GetDistanceTo(player.Character);
                    if (trapDistance < distance)
                        distance = trapDistance;
                }

            trapsByDistance.Add(distance, trap);
        }

        return trapsByDistance;
    }

    public List<KeyValuePair<Trap, SortedList<float, Character>>> GetCharactersInEachTrapRadar(Player exception)
    {
        // Returns a List of Traps (sorted by the closes character to the trap) related with a
        // sorted list of the characters in each trap and the distance from them to it.

        // Order the traps by the distance to the closest character
        // chInTrapsSorted = new SortedList<distanceToClosestChar, KeyValuePair<Trap, sortedListOfTheDistToAllChars>>();
        
        SortedList<float, KeyValuePair<Trap, SortedList<float, Character>>> chInTrapsSorted = new SortedList<float, KeyValuePair<Trap, SortedList<float, Character>>>();
        
        foreach (Trap trap in this)
        {
            SortedList<float, Character> t = trap.GetCharactersInTrapRadar(exception); // sortedListOfTheDistToAllChars
            
            if (t.Count > 0)
                if (!chInTrapsSorted.ContainsKey(t.Keys[0]))
                    chInTrapsSorted.Add(t.Keys[0], new KeyValuePair<Trap, SortedList<float, Character>>(trap, t));
        }

        //Convert to more readable list
        List<KeyValuePair<Trap, SortedList<float, Character>>> charactersInEachTrapRadar =
            new List<KeyValuePair<Trap, SortedList<float, Character>>>();
        foreach (KeyValuePair<float, KeyValuePair<Trap, SortedList<float, Character>>> t in chInTrapsSorted)
            charactersInEachTrapRadar.Add(
                new KeyValuePair<Trap, SortedList<float, Character>>(t.Value.Key, t.Value.Value));

        return charactersInEachTrapRadar;
    }
}