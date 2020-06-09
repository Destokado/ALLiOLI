
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);

        if (!character || character.isDead ) 
            return;
        
        character.ServerSuicide();
    }
}
