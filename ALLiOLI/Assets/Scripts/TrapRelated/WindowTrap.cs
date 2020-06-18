using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class WindowTrap : Trap
{
    private SimpleAnimationsManager animManager;

    protected override void Awake()
    {
        base.Awake();
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
    }
}