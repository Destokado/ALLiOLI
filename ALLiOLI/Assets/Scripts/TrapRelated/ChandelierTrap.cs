using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class ChandelierTrap : Trap
{
    private SimpleAnimationsManager animManager;
    [SerializeField] private Rigidbody chandelier;

    public void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        ((TransformAnimation) animManager.GetAnimation(0)).originTransform.SetProperties(chandelier.transform);
        chandelier.isKinematic = true;
        animManager.Play(0);
    }

    public override void RpcActivate()
    {
        base.RpcActivate();

        animManager.Stop(0);
        chandelier.isKinematic = false;
    }
}