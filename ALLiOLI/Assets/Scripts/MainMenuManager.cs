using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager singleton { get; private set; }
    [SerializeField] private Object matchScene;

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
        Debug.Log("Loading scene " + matchScene.name);
        SceneManager.LoadScene(matchScene.name, LoadSceneMode.Single);
    }
}
