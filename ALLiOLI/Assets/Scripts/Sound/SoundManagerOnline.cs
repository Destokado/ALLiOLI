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


    [ClientRpc]
    private void RpcPlayOneShotOnGameObject(uint netId, int i)
    {
        var gameObject=  ((AllIOliNetworkManager) NetworkManager.singleton).GetGameObject(netId);
        gameObject.GetComponent<SoundEmitterHandler>().Play(i);
    }

    [Command]
    private void CmdPlayOneShotOnGameObject(uint netId, int i)
    {
        RpcPlayOneShotOnGameObject(netId, i);
    }

    public void PlayOneShotOnGameObjectAllClients(uint netId, int i)
    {
        CmdPlayOneShotOnGameObject(netId, i);
    }

    #endregion


    #region PlayEvent

    

   
    /*
    [ClientRpc]
    private void RpcPlayEvent(string path, Vector3 pos)
    {
        soundManager.PlayEventLocal(path, pos);
    }

    [Command]
    private void CmdPlayEvent(string path, Vector3 pos)
    {
        RpcPlayEvent(path, pos);
    }

    public EventInstance PlayEventAllClients(string path, Vector3 pos)
    {
        return soundManager.PlayEventLocal(path, pos);
    }*/


    [ClientRpc]
    private void RpcPlayEventOnGameObject(uint netId, int i)
    {
        
       var gameObject=  ((AllIOliNetworkManager) NetworkManager.singleton).GetGameObject(netId);
       gameObject.GetComponent<SoundEmitterHandler>().Play(i);
    }

    [Command]
    private void CmdPlayEventOnGameObject(uint netId, int i)
    {
         RpcPlayEventOnGameObject(netId, i);
    }

    public void PlayEventOnGameObjectAllClients(uint netId, int i)
    {
         CmdPlayEventOnGameObject(netId, i);
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
    private void RpcStopEventOnGameObject(uint netId, int i)
    {
        SoundManager.Instance.StopEventOnGameObjectLocal(netId, i);
       
    }

    [Command]
    private void CmdStopEventOnGameObject(uint netId, int i)
    {
        RpcStopEventOnGameObject(netId,i);
    }

    public void StopEventOnGameObjectAllClients(uint netId, int i)
    {
        CmdPlayEventOnGameObject(netId,i);
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