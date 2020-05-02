using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class ChandelierTrap : Trap
{
    
    private SimpleAnimationsManager animManager;
    [SerializeField] private KillZone killZone;

    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
    }

    public void DisableKillZone()
    {
        //TODO: CHANGE
        //killZone.SetState(false);
    }
    
}