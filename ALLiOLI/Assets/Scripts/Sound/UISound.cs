
using System;
using UnityEngine;



public class UISound : MonoBehaviour
{
 private SoundManager soundManager;
    // Start is called before the first frame update

    private void Awake()
    {
        soundManager=SoundManager.Instance;
    }

   

    public void OnMouseEnter()
    {
        var path = SoundManager.EventPaths.ButtonHover;
        soundManager.PlayOneShotLocal(path, Vector3.zero, null);
    }

    public void OnButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundManager.EventPaths.Button,Vector3.zero, null);
    }
    public void OnPlayButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundManager.EventPaths.PlayButton,Vector3.zero, null);
    }
    public void OnBackButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundManager.EventPaths.BackButton,Vector3.zero, null);
    }
}