using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Mirror;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Parameters

    private static SoundManager instance;
    private Dictionary<string, EventInstance> eventsList;

    private EventInstance music;

    private List<SoundManagerMovingSound> positionEvents;

    #endregion Parameters

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<SoundManager>();
                instance.name = "SoundManagerOnline";
            }

            return instance;
        }
    }

    void Awake()
    {
        if ((instance != null && instance != this))
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
    }

    private void Init()
    {
        eventsList = new Dictionary<string, EventInstance>();
        positionEvents = new List<SoundManagerMovingSound>();
        PlayEventLocal(EventPaths.Music, Vector3.zero);
    }

    void Update()
    {
        // Actualizamos las posiciones de los sonidos 3D
        if (positionEvents != null && positionEvents.Count > 0)
        {
            for (int i = 0; i < positionEvents.Count; i++)
            {
                PLAYBACK_STATE state;
                EventInstance eventInst = positionEvents[i].GetEventInstance();
                eventInst.getPlaybackState(out state);
                if (state == PLAYBACK_STATE.STOPPED)
                {
                    positionEvents.RemoveAt(i);
                }
                else
                {
                    eventInst.set3DAttributes(RuntimeUtils.To3DAttributes(positionEvents[i].GetTransform().position));
                }
            }
        }
    }

    private EventInstance GetEventFromPath(string path)
    {
        try
        {
            if (eventsList.ContainsKey(path)) return eventsList[path];

        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning($"The event with path {path} wasn't found in the {eventsList} dictionary");
        }
        return new EventInstance();
        //  Debug.LogWarning($"The event with path {path} wasn't found in the {eventsList} dictionary");
    }

    public void PlayOneShotLocal(string path, Vector3 pos, SoundManagerParameter[] parameters)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            if (parameters != null)
                for (int i = 0; i < parameters.Length; i++)
                {
                    soundEvent.setParameterByName(parameters[i].name, parameters[i].value);
                }

            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            soundEvent.release();
        }
        else

            Debug.LogWarning("The event with path: " + path + " is null");
    }

    // Usamos esta para objetos en movimiento que actualizan la posición del sonido
    public void PlayOneShotMovingLocal(string path, Transform transform)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            soundEvent.release();
        }
        else Debug.LogWarning("The event with path: " + path + " is null");
    }

    public EventInstance PlayEventLocal(string path, Vector3 pos)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            if (eventsList.ContainsKey(path))
            {
                Debug.LogWarning(
                    $"Trying to add the event {path} that already exists in the dictionary. Removing old event");
                eventsList.Remove(path);
            }
             

            else
            {
                eventsList.Add(path, soundEvent);
            }
        }
        else
        {
            Debug.LogWarning("The event with path: " + path + " is null");
        }

        return soundEvent;
    }

    // Usamos esta para objetos en movimiento que actualizan la posición del sonido
    public EventInstance PlayEventMovingLocal(string path, Transform transform)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            eventsList.Add(path, soundEvent);
        }
        else
        {
            Debug.LogWarning("The event with path: " + path + " is null");
        }

        return soundEvent;
    }

    public void StopEventOnGameObjectLocal(uint netId, string path)
    {
        var gameObject = ((AllIOliNetworkManager) NetworkManager.singleton).GetGameObject(netId);
        gameObject.GetComponentInChildren<SoundEmitterHandler>().Stop(path);
    }

    public void PlayEventOnGameObjectLocal(uint netId, string path)
    {
        var gameObject = ((AllIOliNetworkManager) NetworkManager.singleton).GetGameObject(netId);
        gameObject.GetComponentInChildren<SoundEmitterHandler>().Play(path);
    }

    public void StopEventLocal(string path, bool fadeout)
    {
        EventInstance soundEvent = GetEventFromPath(path);
        

        if (eventsList.Remove(path))
        {
            if (fadeout)
                soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            Debug.LogWarning("The event : " + soundEvent + " didn't stop");
        }
    }


    public void PauseEventLocal(string path)
    {
        EventInstance soundEvent = GetEventFromPath(path);
        soundEvent.setPaused(true);
    }

    public void ResumeEventLocal(string path)
    {
        EventInstance soundEvent = GetEventFromPath(path);
        soundEvent.setPaused(false);
    }

    public void StopAllEventsLocal(bool fadeout)
    {
        foreach (var pair in eventsList)
        {
            if (fadeout)
                pair.Value.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                pair.Value.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        eventsList.Clear();
    }

    public bool isPlaying(string path)
    {
        try
        {
            EventInstance soundEvent = GetEventFromPath(path);
            PLAYBACK_STATE state;
            soundEvent.getPlaybackState(out state);
            return state.Equals(PLAYBACK_STATE.PLAYING);
        }
        catch (KeyNotFoundException)
        {
            Debug.Log(path + " evnet not found");
            return false;
        }
    }

    // #endregion Events

    #region Mixer

    public void SetChannelVolume(string channel, float channelVolume)
    {
        VCA vca;
        if (RuntimeManager.StudioSystem.getVCA("vca:/" + channel, out vca) != FMOD.RESULT.OK)
            return;
        vca.setVolume(channelVolume);
    }

    #endregion Mixer

    #region ExtraClasses

//Parametro genérico de FMOD para pasar a los eventos

    public readonly struct SoundManagerParameter
    {
        public readonly string name;
        public readonly float value;

        public SoundManagerParameter(string name, float value)
        {
            this.name = name;
            this.value = value;
        }
    }


//Parametro genérico de FMOD para pasar a los eventos
    class SoundManagerMovingSound
    {
        Transform transform;
        EventInstance eventIns;

        public SoundManagerMovingSound(Transform transform, EventInstance eventIns)
        {
            this.transform = transform;
            this.eventIns = eventIns;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public EventInstance GetEventInstance()
        {
            return eventIns;
        }
    }

    public static class EventPaths
    {
        public static string Music = "event:/Music";
        #region Character
        
        public static string Jump = "event:/Character/Jump";
        public static string Run = "event:/Character/Run 2";
        public static string Land = "event:/Character/Land";
        public static string Death = "event:/Character/Death";

        public static string
            WallHit =
                "event:/Character/WallHit"; //TODO: Make the character detect collisions with walls when sent away by traps

        public static string Spawn = "event:/Character/Spawn";
        public static string Win = "event:/Character/Win2";
        public static string Defeat = "event:/Character/Defeat";
        public static string PickUp = "event:/Character/FlagPickup";

        #endregion

        #region Traps

        public static string ActivateTrap= "event:/Trap/Activate";
        public static string Deny="event:/Trap/Deny";
        public static string Alarm ="event:/Trap/Alarm";
        public static string PickActivator ="event:/Trap/PickActivator";
        
        
        public static string BirdShitActivate = "event:/Trap/BirdShitActivate";
        public static string BirdShitHit = "event:/Trap/BirdShitHit";
        public static string BombActivate = "event:/Trap/BombActivate";
        public static string BombReset = "event:/Trap/BombReset";
        public static string ChapaActivate = "event:/Trap/ChapaActivate";
        public static string ChapaHit = "event:/Trap/ChapaHit";
        public static string DragonActivate = "event:/Trap/DragonActivate";
        public static string DragonReset = "event:/Trap/DragonReset";
        public static string PendulumActivate = "event:/Trap/PendulumActivate";
        public static string PendulumReset = "event:/Trap/PendulumReset";
        public static string RollingActivate = "event:/Trap/RollingActivate";
        public static string RollingHit = "event:/Trap/RollingHit";
        public static string SlapActivate = "event:/Trap/SlapActivate";
        public static string SlapHit = "event:/Trap/SlapHit";
        public static string SlapReset = "event:/Trap/SlapReset";
        public static string SpikesActivate = "event:/Trap/SpikesActivate";
        public static string SpikesHit = "event:/Trap/SpikesHit";

        #endregion

        #region UI

        public static string Button = "event:/UI/Button";
        public static string PlayButton = "event:/UI/PlayButton";
        public static string FlagAnnouncement = "event:/UI/FlagAnnounce";
        public static string ButtonHover = "event:/UI/ButtonHover";
        public static string BackButton = "event:/UI/BackButton";
        public static string DeathAnnouncement = "event:/UI/DeathAnnouncement";
        public static string Finish = "event:/UI/Finish";
        public static string CountDown = "event:/UI/RaceCountdown";

        #endregion
    }

    #endregion ExtraClasses
}