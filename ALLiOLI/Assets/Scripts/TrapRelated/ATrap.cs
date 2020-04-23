using UnityEngine;

public abstract class ATrap : MonoBehaviour
{
    public bool placed { get; private set; }

    [SerializeField] private float activatableRange;
    [SerializeField] private float cooldownTime;

    public bool OnCd
    {
        get => cdTimer > 0;
        private set { }
    }

    private float cdTimer;

    private void Start()
    {
        placed = false;
    }

    private void Update()
    {
        cdTimer -= Time.deltaTime;
    }

    public void SetUp()
    {
        placed = !placed;
        Debug.Log("The trap "+gameObject.name+" has been set to:"+placed);

    }

    public virtual void Activate()
    {
        Debug.Log("The trap "+gameObject.name+" has been activated");
        cdTimer = cooldownTime;
    }

    private bool HasCharInRange()
    {
        foreach (Player player in MatchManager.Instance.players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= activatableRange) return true;
        }

        return false;
    }

    public bool IsActivatable()
    {
        return HasCharInRange() && !OnCd;
    }
}