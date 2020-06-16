using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class SimpleAnimationTrap : Trap
{
    // Start is called before the first frame update
    protected SimpleAnimationsManager animManager;
   [SerializeField] protected StudioEventEmitter resetEmitter;

    protected void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        base.Reload();
        if (resetEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, resetEmitter.Event);
        else
        {
            Debug.LogWarning($" The resetEmitter is null in {gameObject.name}");
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