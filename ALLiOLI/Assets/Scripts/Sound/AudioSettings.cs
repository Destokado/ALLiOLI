using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.EventInstance SFXVolumeTestEvent;

   /* [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider FXSlider;
    [SerializeField] private Slider UISlider;*/
    
    private FMOD.Studio.Bus Master;
    private FMOD.Studio.VCA Music;
    private FMOD.Studio.VCA FX;
    private FMOD.Studio.VCA UI;
    
    
    float MusicVolume = 0.5f;
    float FXVolume = 0.5f;
    float MasterVolume = 1f;
    float UIVolume = 1f;
    
    void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        Music = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        FX = FMODUnity.RuntimeManager.GetVCA("vca:/FX");
        UI = FMODUnity.RuntimeManager.GetVCA("vca:/UI");
        
       /* MasterSlider.onValueChanged.AddListener(MasterVolumeLevel);
        MusicSlider.onValueChanged.AddListener(MusicVolumeLevel);
        FXSlider.onValueChanged.AddListener(FXVolumeLevel);
        UISlider.onValueChanged.AddListener(UIVolumeLevel);
        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance (SoundManager.EventPaths.WallHit);*/
        

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
        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState (out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
        {
            SFXVolumeTestEvent.start ();
        }
       
    }
}
