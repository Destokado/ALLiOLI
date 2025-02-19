﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingSceneManager : MonoBehaviour
{
    public static LandingSceneManager singleton { get; private set; }
    //[SerializeField] private Object matchScene;
    [SerializeField] private string matchSceneName;
    private float resetCursorTimer = 10;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("Multiple GameManagers have been created", this);
            return;
        }

        singleton = this;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void StartMatch()
    {
        Debug.Log("Loading scene " + matchSceneName);
        SceneManager.LoadScene(matchSceneName, LoadSceneMode.Single);
    }

    private void Update()
    {
        resetCursorTimer += Time.deltaTime;
        if (resetCursorTimer <= 5) 
            return;

        SetMenuCursor();
        resetCursorTimer = 0;
    }
    
    private void SetMenuCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
