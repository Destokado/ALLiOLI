using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private FMOD.Studio.Bus Master;
    private FMOD.Studio.VCA Music;
    private FMOD.Studio.VCA FX;
    private FMOD.Studio.VCA UI;


    private float MusicVolume;
    private float FXVolume;
    private float MasterVolume;
    private float UIVolume;
    
    void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Music = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        FX = FMODUnity.RuntimeManager.GetVCA("vca:/FX");
        UI = FMODUnity.RuntimeManager.GetVCA("vca:/UI");
        

       MusicVolume = 0.2f;
       FXVolume = 0.7f;
       MasterVolume = .9f;
       UIVolume = .7f;
    }

    private void Start()
    {
        Master.setVolume(MasterVolume);
        Music.setVolume(MusicVolume);
        FX.setVolume(FXVolume);
        UI.setVolume(UIVolume);
    }

    // Update is called once per frame
    void Update()
    {
        Master.setVolume(MasterVolume);
        Music.setVolume(MusicVolume);
        FX.setVolume(FXVolume);
        UI.setVolume(UIVolume);
    }
    public void MasterVolumeLevel (float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
    }
    public void UIVolumeLevel (float newUIVolume)
    {
        UIVolume = newUIVolume;
    }
    public void MusicVolumeLevel (float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
    }

    public void FXVolumeLevel (float newSFXVolume)
    {
        FXVolume = newSFXVolume;
      
       
    }
}
