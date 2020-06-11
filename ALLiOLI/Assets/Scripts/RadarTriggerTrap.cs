using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadarTriggerTrap : MonoBehaviour
{
    private readonly HashSet<Character> charactersInRadar = new HashSet<Character>();
    [SerializeField] private float maximumRadarDistance = 5;
    [SerializeField] private Transform pointOfMaxEffectivity;

    public SortedList<float, Character> GetCharactersInTrapRadar(Player exception) // Sorted by distance
    {
        SortedList<float, Character> charactersWithDiestanceInRadar = new SortedList<float, Character>();

        HashSet<Character> toRemove = new HashSet<Character>();
        foreach (Character character in charactersInRadar)
            if (!character)
                toRemove.Add(character);
            else if (!character.isDead && character.Owner != exception)
                charactersWithDiestanceInRadar.Add(GetRadarDistanceTo(character), character);

        foreach (Character character in toRemove)
            charactersInRadar.Remove(character);
        
        return charactersWithDiestanceInRadar;
    }

    public float GetRadarDistanceTo(Character character)
    {
        return Vector3.Distance(character.transform.position + Vector3.up, pointOfMaxEffectivity.position) /
               maximumRadarDistance;
        //return Vector3.Distance(character.cameraTarget.transform.position, this.gameObject.transform.position)/maximumRadarDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (character) charactersInRadar.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character) charactersInRadar.Remove(character);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Selection.activeGameObject != transform.gameObject) return;

        Gizmos.color = new Color(1f, 0f, 1f, 0.25f);
        Gizmos.DrawSphere(pointOfMaxEffectivity.position, maximumRadarDistance);
    }
#endif
}