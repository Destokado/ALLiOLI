using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundEmitterHandler : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter soundEmitter;
    

    public void Play()
    {
       // soundEmitter.gameObject.SetActive(true);
       if(!soundEmitter.IsPlaying()) soundEmitter.Play();
       
    }

    public void Stop()
    {
       // soundEmitter.gameObject.SetActive(false);
       if (soundEmitter.IsPlaying())
       {
           soundEmitter.Stop();
           soundEmitter.AllowFadeout = true;
       }

    }
}
