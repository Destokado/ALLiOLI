using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CmCamera : MonoBehaviour
{
    private bool initialized = false;

    [SerializeField] private CinemachineFreeLook freeLook;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    public HumanLocalPlayer HumanLocalPlayer
    {
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

    private static readonly Dictionary<string, HumanLocalPlayer> axisNameToHumanInput =
        new Dictionary<string, HumanLocalPlayer>();


    private void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    
    private float GetAxisCustom(string axisName)
    {
        if (axisNameToHumanInput.Count <= 0 || HumanLocalPlayer == null)
            return 0;

        Vector2 lookDelta = axisNameToHumanInput[axisName.Remove(axisName.Length - 1)].CameraMovement;

        switch (axisName.Substring(axisName.Length - 1))
        {
            case "X":
                return lookDelta.x;
            case "Y":
                return lookDelta.y;
        }

        return 0;
    }


    public float SetTargetWithCinematics(Transform target, Transform follow)
    {
        if (!initialized)
        {
            freeLook.Follow = follow;
            freeLook.LookAt = target;
            initialized = true;
            return 0f;
        }
        else
        {
            float blendingTime = cinemachineBrain.m_DefaultBlend.m_Time;
            StartCoroutine(ReSetCamera(target, follow, blendingTime));
            return blendingTime;
        }
    }

    private IEnumerator ReSetCamera(Transform target, Transform follow, float blendingTime)
    {
        freeLook.Priority = 1;
        
        yield return new WaitForSeconds(blendingTime);
        
        freeLook.Follow = follow;
        freeLook.LookAt = target;
        freeLook.Priority = 10;
        
        //TODO: DoRecenter instead? so the recenter time in the inspector is separated from this one
        freeLook.m_YAxisRecentering.RecenterNow(); 
        freeLook.m_RecenterToTargetHeading.RecenterNow();
    }

    public void SetLayer(int layer, Camera cameraComponent)
    {
        cameraComponent.gameObject.SetLayerRecursively(layer);
        cameraComponent.cullingMask |= 1 << layer;
    }
}