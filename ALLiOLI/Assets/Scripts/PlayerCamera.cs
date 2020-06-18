using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] public CinemachineBrain cinemachineBrain;

    public HumanLocalPlayer HumanLocalPlayer
    {
        get => _humanLocalPlayer;
        set
        {
            if (_humanLocalPlayer != null)
            {
                Debug.LogWarning("Trying to reset the HumanLocalPlayer of a PlayerCamera. Shouldn't be done");
                
                axisNameToHumanInput.Remove(_humanLocalPlayer.gameObject.name);
            }

            _humanLocalPlayer = value;

            if (value != null)
            {
                Debug.Log($"PlayerCamera has set the HumanLocalPlayer as {HumanLocalPlayer.gameObject.name} in {gameObject.name}", gameObject);
                GameObject humanGameObject = value.gameObject;
                axisNameToHumanInput.Add(humanGameObject.name, value);
            }
            else
            {
                Debug.LogWarning($"Setting a null HumanLocalPlayer in {gameObject.name}", gameObject);
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
        {
            //Debug.LogWarning($"HUMANLOCALPLAYER IS {HumanLocalPlayer} and axisNameToHumanInput.Count is {axisNameToHumanInput.Count}", gameObject);
            return 0;
        }

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
    
    public void SetLayer(int layer, Camera cameraComponent)
    {
        cameraComponent.gameObject.SetLayerRecursively(layer);
        cameraComponent.cullingMask |= 1 << layer;
    }


    /*public float SetTargetWithCinematics(Transform target, Transform follow)
    {
        
        if (!initialized)
        {
            HumanLocalPlayer.Player.Character.freeLookCamera.Follow = follow;
            HumanLocalPlayer.Player.Character.freeLookCamera.LookAt = target;
            initialized = true;
            return 0f;
        }
        else
        {
            Vector3 oldPos =  freeLook.LookAt.position;
            float blendingTime = GetDistanceBlendingTime(oldPos);
            Debug.Log(blendingTime);
            cinemachineBrain.m_DefaultBlend.m_Time = blendingTime;
            StartCoroutine(ReSetCamera(target, follow, blendingTime));
            return blendingTime;
        }

    }*/

    /*private float GetDistanceBlendingTime(Vector3 startPos)
    {
        Debug.DrawLine(startPos,Spawner.Instance.transform.position,Color.red,10);
        return blendVelocity*0.05f * Vector3.Distance(startPos, Spawner.Instance.transform.position);
    }

    private IEnumerator ReSetCamera(Transform target, Transform follow, float blendingTime)
    {
        //freeLook.Priority = 1;

        yield return new WaitForSeconds(blendingTime);
        Debug.Log("BLENDING FINISHED after " + blendingTime);
        freeLook.Follow = follow;
        freeLook.LookAt = target;
        //freeLook.Priority = 10;
        
        //TODO: DoRecenter instead? so the recenter time in the inspector is separated from this one
        freeLook.m_YAxisRecentering.RecenterNow();
        freeLook.m_RecenterToTargetHeading.RecenterNow();
        yield return new WaitForEndOfFrame();
    }


    public void ForceSetLookAt(Transform target)
    {
        freeLook.LookAt = target;
    }*/
}