using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CmCamera : MonoBehaviour
{
    
    private Vector2 LookDelta;
    [SerializeField] private CinemachineFreeLook freeLook;

    public HumanLocalPlayer HumanLocalPlayer {
        get => _humanLocalPlayer;
        set
        {
            if (_humanLocalPlayer != null)
                axisNameToHumanInput.Remove(_humanLocalPlayer.gameObject.name);
            
            _humanLocalPlayer = value;

            if (value != null)
            {
                GameObject humanGameObject = value.gameObject;
                freeLook.m_XAxis.m_InputAxisName = humanGameObject.name + "X";
                freeLook.m_YAxis.m_InputAxisName = humanGameObject.name + "Y";
                axisNameToHumanInput.Add(humanGameObject.name, value);
            }
        }
    }
    // ReSharper disable once InconsistentNaming
    private HumanLocalPlayer _humanLocalPlayer;

    private static readonly Dictionary<string, HumanLocalPlayer> axisNameToHumanInput = new Dictionary<string, HumanLocalPlayer>();

    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float GetAxisCustom(string axisName)
    {
        if (axisNameToHumanInput.Count <= 0)
            return 0;
        
        Vector2 lookDelta = axisNameToHumanInput[axisName.Remove(axisName.Length - 1)].cameraMovement;
        
        lookDelta.Normalize(); //TODO: needed?
 
        switch (axisName.Substring(axisName.Length - 1))
        {
            case "X":
                return lookDelta.x;
            case "Y":
                return lookDelta.y;
        }
        
        return 0;
    }


    public void SetTarget(Transform target, Transform follow)
    {
        freeLook.Follow = follow;
        freeLook.LookAt = target;
    }

    public void SetLayer(int layer, Camera cameraComponent)
    {
        cameraComponent.gameObject.SetLayerRecursively(layer); 
        cameraComponent.cullingMask |= 1 << layer;
    }
}
