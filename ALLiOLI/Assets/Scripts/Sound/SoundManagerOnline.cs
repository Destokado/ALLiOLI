using System;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SoundManagerOnline : NetworkBehaviour
{
    #region Initialization

    private SoundManager soundManager;

    #endregion Initialization

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    // #region FMOD Wrapper

    //  #region Events

    // Usamos esta para objetos con parámetros


    [ClientRpc]
    public void RpcPlayOneShot(string path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        soundManager.PlayOneShotLocal(path, pos, parameters);
    }

    [Command]
    private void CmdPlayOneShot(string path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        RpcPlayOneShot(path, pos, parameters);
    }

    public void PlayOneShotAllClients(String path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        CmdPlayOneShot(path, pos, parameters);
    }


    [ClientRpc]
    public void RpcPlayOneShotMoving(string path, Transform transform)
    {
        soundManager.PlayOneShotMovingLocal(path, transform);
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


    [ClientRpc]
    public void RpcPlayEvent(string path, Vector3 pos)
    {
        soundManager.PlayEventLocal(path, pos);
    }

    [Command]
    public void CmdPlayEvent(string path, Vector3 pos)
    {
        RpcPlayEvent(path, pos);
    }

    public EventInstance PlayEventAllClients(string path, Vector3 pos)
    {
        return soundManager.PlayEventLocal(path, pos);
    }


    [ClientRpc]
    public void RpcPlayEventMoving(string path, Transform transform)
    {
        soundManager.PlayEventMovingLocal(path, transform);
    }

    [Command]
    private void CmdPlayEventMoving(string path, Transform transform)
    {
        RpcPlayEventMoving(path, transform);
    }

    public EventInstance PlayEventMovingAllClients(string path, Transform transform)
    {
        return soundManager.PlayEventMovingLocal(path, transform);
    }


    //TODO: Maybe also With CMD and RPC?
    public void UpdateEventParameter(EventInstance soundEvent, SoundManager.SoundManagerParameter parameter)
    {
        soundEvent.setParameterByName(parameter.name, parameter.value);
    }

    public void UpdateEventParameters(EventInstance soundEvent,
        List<SoundManager.SoundManagerParameter> parameters)
    {
        for (int i = 0; i < parameters.Count; i++)
            soundEvent.setParameterByName(parameters[i].name, parameters[i].value);
    }


    [ClientRpc]
    public void RpcStopEvent(String path, bool fadeout)
    {
        soundManager.StopEventLocal(path, fadeout);
    }

    [Command]
    private void CmdStopEvent(String path, bool fadeout)
    {
        RpcStopEvent(path, fadeout);
    }

    public void StopEventAllClients(String path, bool fadeout)
    {
        CmdStopEvent(path, fadeout);
    }


    [ClientRpc]
    public void RpcPauseEvent(String path)
    {
        soundManager.PauseEventLocal(path);
    }

    [Command]
    private void CmdPauseEvent(String path)
    {
        RpcPauseEvent(path);
    }

    public void PauseEventAllClients(String path)
    {
        soundManager.PauseEventLocal(path);
    }


    [ClientRpc]
    public void RpcResumeEvent(String path)
    {
        soundManager.ResumeEventLocal(path);
    }

    [Command]
    private void CmdResumeEvent(String path)
    {
        RpcResumeEvent(path);
    }

    public void ResumeEventAllClients(String path)
    {
        soundManager.ResumeEventLocal(path);
    }


    [ClientRpc]
    public void RpcStopAllEvents(bool fadeout)
    {
        soundManager.StopAllEventsLocal(fadeout);
    }

/*
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
    }*/


    //  #endregion FMOD Wrapper
}