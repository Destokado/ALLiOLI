using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGuiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text trapsCounter;

    public void SetCurrentNumberOfTraps(int currentTraps, int maxNumberOfTraps)
    {
        trapsCounter.text = currentTraps+"/"+maxNumberOfTraps;
    }
}
