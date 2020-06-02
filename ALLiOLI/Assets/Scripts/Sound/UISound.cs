
using System;
using UnityEngine;



public class UISound : MonoBehaviour
{
 [SerializeField]private SoundManager soundManager;
    // Start is called before the first frame update
    

    public void OnMouseEnter()
    {
        var path = SoundEventPaths.buttonHoverPath;
        soundManager.PlayOneShotLocal(path, Vector3.zero, null);
    }

    public void OnButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundEventPaths.buttonPath,Vector3.zero, null);
    }
    public void OnPlayButtonPressed()
    {
        soundManager.PlayOneShotLocal(SoundEventPaths.playButtonPath,Vector3.zero, null);
    }
}