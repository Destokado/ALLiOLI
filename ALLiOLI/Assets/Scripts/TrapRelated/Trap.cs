using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] private float activatableRange;
    [SerializeField] private float cooldownTime;

    protected bool OnCd { get => cdTimer > 0; }

    private float cdTimer;

    private void Update()
    {
        if (cdTimer > 0)
            cdTimer -= Time.deltaTime;
    }

    public virtual void Activate()
    {
        Debug.Log("The trap "+gameObject.name+" has been activated.");
        cdTimer = cooldownTime;
    }

    private bool HasCharacterInActionZone()
    {
        //TODO: keep track if any alive character is inside the "action zone"
        
        /*foreach (Player player in MatchManager.Instance.players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= activatableRange) return true;
        }*/

        return false;
    }

    public bool ItActivatable()
    {
        return !OnCd;
    }

    public float GetDistanceTo(Character character)
    {
        if (HasCharacterInActionZone())
            return 0f;
        
        //return Vector3.Distance(character.transform.position, this.gameObject.transform.position);
        return Vector3.Distance(character.cameraTarget.transform.position, this.gameObject.transform.position);
    }
}