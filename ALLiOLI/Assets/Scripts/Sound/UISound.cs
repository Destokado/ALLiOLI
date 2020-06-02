
using UnityEngine;


public class UISound : MonoBehaviour
{
    private SoundManager instance;
    // Start is called before the first frame update
    public void OnMouseEnter()
    { 
        instance = SoundManager.instance;
        var path = SoundEventPaths.buttonHoverPath;
        instance.PlayOneShotLocal(path, Vector3.zero, null);
    }

    public void OnButtonPressed()
    {
        instance.PlayOneShotLocal(SoundEventPaths.buttonPath,Vector3.zero, null);
    }
    public void OnPlayButtonPressed()
    {
        instance.PlayOneShotLocal(SoundEventPaths.playButtonPath,Vector3.zero, null);
    }
}