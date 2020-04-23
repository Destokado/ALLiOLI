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
    }

    public virtual void Activate()
    {
        cdTimer = cooldownTime;
    }

    private bool HasCharInRange()
    {
        return TrapManager.Instance.HasCharInRange(transform.position, activatableRange);
    }

    public bool IsActivatable()
    {
        return HasCharInRange() && !OnCd;
    }
}