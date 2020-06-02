
using UnityEngine;


public class UISound : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnMouseEnter()
    {
        var instance = SoundManager.instance;
        var path = SoundEventPaths.buttonHoverPath;
        instance.PlayOneShotLocal(path, Vector3.zero, null);
    }
}