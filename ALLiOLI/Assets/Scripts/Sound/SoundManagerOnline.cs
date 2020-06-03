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

    #region OneShot

    [ClientRpc]
    private void RpcPlayOneShotOnPos(string path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        soundManager.PlayOneShotLocal(path, pos, parameters);
    }

    [Command]
    private void CmdPlayOneShotOnPos(string path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        RpcPlayOneShotOnPos(path, pos, parameters);
    }
    
    public void PlayOneShotOnPosAllClients(string path, Vector3 pos, SoundManager.SoundManagerParameter[] parameters)
    {
        CmdPlayOneShotOnPos(path, pos, parameters);
    }

    

    #endregion


    #region PlayEvent
    [ClientRpc]
    private void RpcPlayEventInPos(string path, Vector3 pos)
    {
        soundManager.PlayEventLocal(path, pos);
    }

    [Command]
    private void CmdPlayEventInPos(string path, Vector3 pos)
    {
        RpcPlayEventInPos(path, pos);
    }

    public EventInstance PlayEventInPosAllClients(string path, Vector3 pos)
    {
        return soundManager.PlayEventLocal(path, pos);
    }


    [ClientRpc]
    private void RpcPlayEventOnGameObject(uint netId, string path)
    {
        SoundManager.Instance.PlayEventOnGameObjectLocal(netId,path);
      
    }

    [Command]
    private void CmdPlayEventOnGameObject(uint netId, string path)
    {
         RpcPlayEventOnGameObject(netId, path);
    }

    public void PlayEventOnGameObjectAllClients(uint netId, string path)
    {
         CmdPlayEventOnGameObject(netId, path);
    }
    #endregion

    #region UpdateParameters

    

    
    /// <summary>
    /// Update the event parameter. //TODO: NOT TESTED
    /// </summary>
    /// <param name="soundEvent"> EventIstance to modify</param>
    /// <param name="parameter"> SoundManagerParameter with the new values</param>
    public void UpdateEventParameter(EventInstance soundEvent, SoundManager.SoundManagerParameter parameter)
    {
        soundEvent.setParameterByName(parameter.name, parameter.value);
    }
    /// <summary>
    /// Update the event parameters. //TODO: NOT TESTED
    /// </summary>
    public void UpdateEventParameters(EventInstance soundEvent,
        List<SoundManager.SoundManagerParameter> parameters)
    {
        for (int i = 0; i < parameters.Count; i++)
            soundEvent.setParameterByName(parameters[i].name, parameters[i].value);
    }
    #endregion

    #region Stop

    
    [ClientRpc]
    private void RpcStopEvent(String path, bool fadeout)
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
    private void RpcStopEventOnGameObject(uint netId, string path)
    {
        SoundManager.Instance.StopEventOnGameObjectLocal(netId, path);
       
    }

    [Command]
    private void CmdStopEventOnGameObject(uint netId, string path)
    {
        RpcStopEventOnGameObject(netId,path);
    }

    public void StopEventOnGameObjectAllClients(uint netId, string path)
    {
        CmdPlayEventOnGameObject(netId,path);
    }
    
    
    #endregion
  

/*
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