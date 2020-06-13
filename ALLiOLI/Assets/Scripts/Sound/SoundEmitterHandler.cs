using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using static System.String;

public class SoundEmitterHandler : MonoBehaviour
{
     private List< StudioEventEmitter> soundEmitter;

    private void Awake()
    {
        soundEmitter = GetComponentsInChildren<StudioEventEmitter>().ToList();

    }

    public void Play(string path)
    {
        foreach (var emitter in soundEmitter)
        {
            if (Compare(emitter.Event, path, StringComparison.Ordinal) == 0 && !emitter.IsPlaying())
            {
                emitter.Play();
            }
        }
     
        
       
    }

    public void Stop(string path)
    {
       
        foreach (var emitter in soundEmitter)
        {
            if (Compare(emitter.Event, path, StringComparison.Ordinal) == 0 && emitter.IsPlaying())
            {
                emitter.Stop();
            }
        }

    }

   
}
