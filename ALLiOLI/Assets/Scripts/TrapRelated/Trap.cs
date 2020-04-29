using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    public bool OnCd => cdTimer > 0;
    public float cdTimer { get; private set; }

    private void Update()
    {
        if (OnCd) {
            cdTimer -= Time.deltaTime;
            if (!OnCd)
                Reload();
        }
    }

    protected abstract void Reload();

    public virtual void Activate()
    {
        Debug.Log("The trap "+gameObject.name+" has been activated.");
        cdTimer = cooldownTime;
    }

    private bool HasCharacterInRange()
    {
        //TODO: keep track if any alive character is inside the "action zone"

        return false;
    }

    public bool IsActivatable()
    {
        return !OnCd;
    }

    public float GetDistanceTo(Character character)
    {
        if (HasCharacterInRange())
            return 0f;
        
        return Vector3.Distance(character.transform.position+Vector3.up, this.gameObject.transform.position);
        //return Vector3.Distance(character.cameraTarget.transform.position, this.gameObject.transform.position);
    }
}