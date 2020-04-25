using UnityEngine;

public class Flag : MonoBehaviour
{
    public Player owner { get; private set; }
    public Player carrier { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Spawner>() && owner) 
            MatchManager.Instance.MatchFinished(owner);
        
        if (carrier) 
            return;
        
        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead) 
            return;
        
        Debug.Log("FLAG BY " + other.gameObject.name);

        character.flag = this;
        owner = character.owner;
        carrier = owner;

        Transform tr = transform;
        tr.SetProperties(character.flagPosition);
        tr.parent = character.flagPosition;
    }

    public void Detach()
    {
        carrier = null;
        transform.parent = null;
    }
}