using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) return;
        if (character.hasFlag) //If the character has the flag, reset it
        {
            character.hasFlag = false; //Tell the character they no longer have the flag (deactivate placeholder trap)
            
            character.Owner.Flag.Reset(); //reset it
        }
        Debug.Log($"Killed '{character.name}', was caught trying to get out of the map!'.", gameObject);
        character.ServerSuicide();
        Destroy(other.gameObject, 3f);
    }
}