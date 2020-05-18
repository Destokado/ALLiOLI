using UnityEngine;

public class Flag : MonoBehaviour
{
    private Collider flagCollider;
    public Player owner { get; private set; } //Last player that carried the flag
    public Character carrier { get; private set; } //Currently carrying the flag

    private void Start()
    {
        flagCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (carrier) return;


        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead)
            return;

        Debug.Log("FLAG BY " + other.gameObject.name);

        character.flag = this;
        owner = character.Owner;
        carrier = character;
        flagCollider.enabled = false;

        Transform tr = transform;
        tr.SetProperties(character.flagPosition.position, character.flagPosition.rotation);
        tr.parent = character.flagPosition;
    }

    public void Detach()
    {
        flagCollider.enabled = true;
        carrier.flag = null;
        carrier = null;
        transform.parent = null;
    }
}