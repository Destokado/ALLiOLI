using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public static TrapManager Instance { get; private set; }
    private List<ATrap> traps;
    private List<Character> characters;
   private void Awake()
      {
          if (Instance != null)
          {
              Debug.LogWarning("Multiple TrapManagers have been created. Destroying the script of " + gameObject.name, gameObject);
              Destroy(this);
          }
          else
          {
              Instance = this;
          }
      }

   private void Start()
   {
       traps = FindObjectsOfType<ATrap>().ToList();
       foreach (Player player in MatchManager.Instance.players)
       {
           characters.Add(player.GetComponent<Character>());
       }
   }

   public ATrap GetClosestTrap(Vector3 charPos)
    {
        ATrap closestTrap = traps[1];
        foreach (ATrap trap in traps)
        {
            if (Vector3.Distance(trap.transform.position, charPos) <
                Vector3.Distance(closestTrap.transform.position, charPos))
                closestTrap = trap;
        }

        return closestTrap;
    }

    public bool HasCharInRange(Vector3 trapPos, float activatableRange)
    {
        foreach (Character character in characters)
        {
            if (Vector3.Distance(character.transform.position, trapPos) <= activatableRange) return true;
        }

        return false;
    }
}
