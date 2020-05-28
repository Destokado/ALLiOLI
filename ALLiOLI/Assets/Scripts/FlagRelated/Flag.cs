using Mirror;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player owner { get; private set; } //Last player that carried the flag
    public Character carrier { get; private set; } //Currently carrying the flag

    private void OnTriggerEnter(Collider other)
    {
        if (carrier) return;
        if (!isServer) return;

        Character character = other.GetComponentInParent<Character>();
        if (!character || character.isDead)
            return;

        owner = character.Owner;
        carrier = character;

        carrier.hasFlag = true;
        
        gameObject.SetActive(false);
    }

    [Server]
    public void Detach()
    {
        // TODO: move to detach position
        
        carrier = null;
        owner = null;

        gameObject.SetActive(true);
    }
}