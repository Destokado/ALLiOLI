using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class DragonTrap : Trap
{
    [FormerlySerializedAs("animManager")] [SerializeField] private SimpleAnimationsManager fireAnim;
   [FormerlySerializedAs("secondanimManager")] [SerializeField] private SimpleAnimationsManager jawAnim;

   [SerializeField] private StudioEventEmitter reloadEmitter;
    private void Awake()
    {
        // fireAnim = gameObject.GetComponent<SimpleAnimationsManager>();
       // jawAnim = gameObject.GetComponentInChildren<SimpleAnimationsManager>();

    }

    protected override void Reload()
    {
        base.Reload();
        if (reloadEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, reloadEmitter.Event);
        fireAnim.GetAnimation(0).mirror = true;
        jawAnim.GetAnimation(0).mirror = true;
        fireAnim.Play(0);
        jawAnim.Play(0);
    }

    public override void Activate()
    {
        base.Activate();
        fireAnim.GetAnimation(0).mirror = false;
        jawAnim.GetAnimation(0).mirror = false;
        fireAnim.Play(0);
        jawAnim.Play(0);
    }
}