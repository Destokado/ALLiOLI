
using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class SwordsTrap : Trap
{
    
    private SimpleAnimationsManager animManager;

    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        animManager.GetAnimation(0).mirror = true;
        animManager.GetAnimation(1).mirror = true;

        animManager.Play(0);
        animManager.Play(1);
        
    }

    public override void Activate()
    {
        base.Activate();
        animManager.GetAnimation(0).mirror = false;
        animManager.GetAnimation(1).mirror = false;
        animManager.Play(0);
        animManager.Play(1);
    }
    
}

