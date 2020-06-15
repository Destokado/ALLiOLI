using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class SimpleAnimationTrap : Trap
{
    // Start is called before the first frame update
    private SimpleAnimationsManager animManager;

    protected void Awake()
    {
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