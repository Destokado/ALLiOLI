using UnityEngine;

public class MapBoundries : MonoBehaviour
{
    public static float KillZoneHeight => -10;
    public static float DeactivationZoneHeight => -150;
    
    /*private void OnTriggerExit(Collider other)
    {
        Flag flag = other.GetComponent<Flag>();
        if (flag && flag.hasAuthority) //If the flag falls off the map, reset it
        {
            Debug.Log($"The flag of the player {flag.Owner.name} has fallen into the void, resetting it");
            flag.Reset();
            return;
        }

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead || !character.Owner.Client.isLocalClient) 
            return;
        character.Kill( Vector3.zero, character.transform.position);
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
    }*/

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(Vector3.up*DeactivationZoneHeight, new Vector3(120, 0, 120));
        
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(Vector3.up*KillZoneHeight, new Vector3(100, 0, 100));
    }
    
}