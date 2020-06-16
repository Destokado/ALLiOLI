using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class SimpleAnimationTrap : Trap
{
    // Start is called before the first frame update
    private SimpleAnimationsManager animManager;
    [SerializeField] private StudioEventEmitter reloadEmitter;

    protected void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        if (reloadEmitter.Event != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, reloadEmitter.Event);
        else
        {
            Debug.LogWarning($" The reloadEmitter.Event  is null in {gameObject.name}");
        }
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