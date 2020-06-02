using System;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SoundManager : NetworkBehaviour
{
    #region Parameters

    public static SoundManager instance { get; private set; }

    private Dictionary<String, EventInstance> eventsList;

    private EventInstance music;

    private List<SoundManagerMovingSound> positionEvents;

    #endregion Parameters

    #region Initialization

   /* public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("getinstance called (null)");
                GameObject go = new GameObject();
                instance = go.AddComponent<SoundManager>();
                instance.name = "SoundManager";
            }
            Debug.Log("getInstance called (NOT NULL)");
            return instance;
        }
    }*/

    private void Awake()
    {
        if ((instance != null && instance != this))
        {
            Debug.LogWarning("More than one SoundManager created. Deleting the previous one.", this);
            Destroy(instance);
        }

        instance = this;
        Init();
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

    private void Init()
    {
        eventsList = new Dictionary<string, EventInstance>();
        positionEvents = new List<SoundManagerMovingSound>();
    }

    #endregion Initialization


    // #region FMOD Wrapper

    //  #region Events

    // Usamos esta para objetos con parámetros
    public void PlayOneShotLocal(string path, Vector3 pos, SoundManagerParameter[] parameters)
    {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null))
        {
            if (parameters != null)
                for (int i = 0; i < parameters.Length; i++)
                    soundEvent.setParameterByName(parameters[i].name, parameters[i].value);

            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            soundEvent.release();
        }
        else

            Debug.LogWarning("The event with path: " + path + " is null");
    }

    [ClientRpc]
    public void RpcPlayOneShot(string path, Vector3 pos, SoundManagerParameter[] parameters)
    {
        PlayOneShotLocal(path, pos, parameters);
    }

    [Command]
    private void CmdPlayOneShot(string path, Vector3 pos, SoundManagerParameter[] parameters)
    {
        RpcPlayOneShot(path, pos, parameters);
    }

    public void PlayOneShotAllClients(String path, Vector3 pos, SoundManagerParameter[] parameters)
    {
        CmdPlayOneShot(path, pos, parameters);
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

    [ClientRpc]
    public void RpcPlayOneShotMoving(string path, Transform transform)
    {
        PlayOneShotMovingLocal(path, transform);
    }

    [Command]
    public void CmdPlayOneShotMoving(string path, Transform transform)
    {
        RpcPlayOneShotMoving(path, transform);
    }

    public void PlayOneShotMovingAllClients(String path, Transform transform)
    {
        CmdPlayOneShotMoving(path, transform);
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

    [ClientRpc]
    public void RpcPlayEvent(string path, Vector3 pos)
    {
        PlayEventLocal(path, pos);
    }

    [Command]
    public void CmdPlayEvent(string path, Vector3 pos)
    {
        RpcPlayEvent(path, pos);
    }

    public EventInstance PlayEventAllClients(string path, Vector3 pos)
    {
        return PlayEventLocal(path, pos);
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

    [ClientRpc]
    public void RpcPlayEventMoving(string path, Transform transform)
    {
        PlayEventMovingLocal(path, transform);
    }

    [Command]
    private void CmdPlayEventMoving(string path, Transform transform)
    {
        RpcPlayEventMoving(path, transform);
    }

    public EventInstance PlayEventMovingAllClients(string path, Transform transform)
    {
        return PlayEventMovingLocal(path, transform);
    }


    //TODO: Maybe also With CMD and RPC?
    public void UpdateEventParameter(EventInstance soundEvent, SoundManagerParameter parameter)
    {
        soundEvent.setParameterByName(parameter.name, parameter.value);
    }

    public void UpdateEventParameters(EventInstance soundEvent, List<SoundManagerParameter> parameters)
    {
        for (int i = 0; i < parameters.Count; i++)
            soundEvent.setParameterByName(parameters[i].name, parameters[i].value);
    }

    public void StopEventLocal(String path, bool fadeout)
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


    [ClientRpc]
    public void RpcStopEvent(String path, bool fadeout)
    {
        StopEventLocal(path, fadeout);
    }

    [Command]
    private void CmdStopEvent(String path, bool fadeout)
    {
        RpcStopEvent(path, fadeout);
    }

    public void StopEventAllClients(String path, bool fadeout)
    {
        StopEventLocal(path, fadeout);
    }


    public void PauseEventLocal(String path)
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

    [ClientRpc]
    public void RpcPauseEvent(String path)
    {
        PauseEventLocal(path);
    }

    [Command]
    private void CmdPauseEvent(String path)
    {
        RpcPauseEvent(path);
    }

    public void PauseEventAllClients(String path)
    {
        PauseEventLocal(path);
    }

    public void ResumeEventLocal(String path)
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

    [ClientRpc]
    public void RpcResumeEvent(String path)
    {
        ResumeEventLocal(path);
    }

    [Command]
    private void CmdResumeEvent(String path)
    {
        RpcResumeEvent(path);
    }

    public void ResumeEventAllClients(String path)
    {
        ResumeEventLocal(path);
    }

    public void StopAllEvents(bool fadeout)
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

    [ClientRpc]
    public void RpcStopAllEvents(bool fadeout)
    {
        StopAllEvents(fadeout);
    }


    public void PauseAllEvents()
    {
        foreach (var pair in eventsList)
        {
            pair.Value.setPaused(true);
        }
    }

    public void ResumeAllEvents()
    {
        foreach (var pair in eventsList)
        {
            pair.Value.setPaused(false);
        }
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

    //  #endregion FMOD Wrapper
}


#region ExtraClasses

//Parametro genérico de FMOD para pasar a los eventos

public readonly struct SoundManagerParameter 
{
    public string name { get; }
    public float value { get; }

    public SoundManagerParameter(string name, float value)
    {
        this.name = name;
        this.value = value;
    }

/*
    public bool Equals(SoundManagerParameter other)
    {
        return name == other.name && value.Equals(other.value);
    }

    public override bool Equals(object obj)
    {
        return obj is SoundManagerParameter other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((name != null ? name.GetHashCode() : 0) * 397) ^ value.GetHashCode();
        }
    }*/
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
    public static string runPath;
    public static string activateTrapPath;
    public static string landPath;
    public static string deathPath;
    public static string punchPath;
    public static string wallHitPath;
    public static string spawnPath;
    public static string winPath;
    public static string defeatPath;
    public static string finishPath;
    public static string buttonCooldownPath;
    public static string pickUpPath;
    public static string flagAnnouncePath;
    public static string playButtonPath = "event:/PlayButton";
    public static string buttonHoverPath = "event:/ButtonHover";
}

#endregion ExtraClasses