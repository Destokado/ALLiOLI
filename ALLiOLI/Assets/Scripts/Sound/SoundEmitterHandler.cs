using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundEmitterHandler : MonoBehaviour
{
    [SerializeField] private List< StudioEventEmitter> soundEmitter;
    

    public void Play(int i)
    {
        if(!soundEmitter[i].IsPlaying())  soundEmitter[i].Play();
        
       
    }

    public void Stop(int i)
    {
       
       if (soundEmitter[i].IsPlaying())
       {
           soundEmitter[i].Stop();
       }

    }
}
