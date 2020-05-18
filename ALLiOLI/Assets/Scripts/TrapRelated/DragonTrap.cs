using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class DragonTrap : Trap
{
    private SimpleAnimationsManager animManager;

    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void RpcActivate()
    {
        base.RpcActivate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
    }
}