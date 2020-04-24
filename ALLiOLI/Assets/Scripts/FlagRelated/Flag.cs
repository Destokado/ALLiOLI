using UnityEngine;

public class Flag : MonoBehaviour
{
    public Player owner { get; private set; }
    public Player carrier { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Spawner>() && owner) MatchManager.Instance.MatchFinished(owner);
        if (carrier) return;
        Character ch = other.GetComponentInParent<Character>();
        if (!ch) return;
        if(ch.isDead) return;
        ch.flag = this;
        owner = ch.owner;
        carrier = owner;

        Transform tr = transform;
        tr.position = ch.flagPosition.position;
        tr.rotation = ch.transform.rotation;
        tr.parent = ch.flagPosition;
    }

    public void Detach()
    {
        carrier = null;
        transform.parent = null;
    }
}