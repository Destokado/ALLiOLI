using Mirror;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class AlfombraTrap : Trap
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

    [ClientRpc]
    public override void RpcActivate()
    {
        base.RpcActivate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
    }
}