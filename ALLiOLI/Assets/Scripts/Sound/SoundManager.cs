using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
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
            DestroyObject(this.gameObject);
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
            eventsList.Add(path, soundEvent);
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

    public void StopEventLocal(string path, bool fadeout)
    {
        //TODO: PASS A WAY TO FIND THE EVENT, NOT THE EVENT (MIRROR UNSUPORTED)
        //soundEvent.clearHandle();
        EventInstance soundEvent = eventsList[path];
        if (eventsList.Remove(path))
        {
            if (fadeout)
                soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            Debug.LogWarning("The event : " + soundEvent + " is null or is not on the list");
        }
    }

    public void PauseEventLocal(string path)
    {
        if (eventsList.ContainsKey(path))
        {
            EventInstance soundEvent = eventsList[path];
            soundEvent.setPaused(true);
        }
        else
        {
            Debug.LogWarning("The event : " + path + " is null or is not on the list");
        }
    }

    public void ResumeEventLocal(string path)
    {
        if (eventsList.ContainsKey(path))
        {
            EventInstance soundEvent = eventsList[path];

            soundEvent.setPaused(false);
        }
        else
        {
            Debug.LogWarning("The event : " + path + " is null or is not on the list");
        }
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

    public bool isPlaying(EventInstance soundEvent)
    {
        PLAYBACK_STATE state;
        soundEvent.getPlaybackState(out state);
        return !state.Equals(PLAYBACK_STATE.STOPPED);
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

    public static class SoundEventPaths
    {
        public static string jumpPath = "event:/Jump";
        public static string runPath = "event:/Run";
        public static string activateTrapPath;
        public static string landPath = "event:/Land";
        public static string deathPath;
        public static string punchPath;
        public static string wallHitPath;
        public static string spawnPath;
        public static string winPath;
        public static string defeatPath;
        public static string finishPath;
        public static string buttonCooldownPath;
        public static string pickUpPath = "event:/FlagPickup";
        public static string flagAnnouncePath;
        public static string playButtonPath = "event:/PlayButton";
        public static string buttonHoverPath = "event:/ButtonHover";
        public static string buttonPath = "event:/Button";
    }

    #endregion ExtraClasses
}