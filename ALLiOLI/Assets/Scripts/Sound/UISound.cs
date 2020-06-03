
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
        var path = SoundManager.SoundEventPaths.buttonHoverPath;
        soundManager.PlayOneShotLocal(path, Vector3.zero, null);
    }

    public void OnButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundManager.SoundEventPaths.buttonPath,Vector3.zero, null);
    }
    public void OnPlayButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundManager.SoundEventPaths.playButtonPath,Vector3.zero, null);
    }
}