using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class DragonTrap : Trap
{

    

   [SerializeField] private StudioEventEmitter resetEmitter;
   [SerializeField] private SimpleAnimationsManager animManager;


    protected override void Reload()
    {
        base.Reload();

        if (resetEmitter != null)
            Client.LocalClient.SoundManagerOnline.PlayEventOnGameObjectAllClients(netId, resetEmitter.Event);
        

        animManager.GetAnimation(0).mirror = true;
        animManager.GetAnimation(1).mirror = true;
        animManager.Play(0);
        animManager.Play(1);

    }

    public override void Activate()
    {
        base.Activate();
        animManager.GetAnimation(1).mirror = false;
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(1);
        animManager.Play(0);
        
    }
}