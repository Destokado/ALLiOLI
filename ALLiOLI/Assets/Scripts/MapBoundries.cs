using UnityEngine;

public class MapBoundries : MonoBehaviour
{
    public static float HeavenHeight = 20;
    public static float HeavenAttraction = 8.75f;
    public static float KillZoneHeight => -10;
    public static float DeactivationZoneHeight => -150;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 0.0f, 0.0f, .42f);
        Gizmos.DrawCube(Vector3.up*DeactivationZoneHeight, new Vector3(150, 0, 400));
        
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, .42f);
        Gizmos.DrawCube(Vector3.up*KillZoneHeight, new Vector3(100, 0, 3));
        
        Gizmos.color = new Color(0.0f, 0.85f, 0.75f, 0.42f);
        Gizmos.DrawCube(Vector3.up*HeavenHeight, new Vector3(100, 0, 300));
    }
    
}