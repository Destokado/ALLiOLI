using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKFootPlacement : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float footBoneDistanceToGround = 0.05f;
    [SerializeField] private float maxFootElevation = 1f;
    [SerializeField] private LayerMask walkableLayers;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
        
        //Left foot
        RaycastHit hit;
        Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot)+Vector3.up*maxFootElevation, Vector3.down);
        if (Physics.Raycast(ray, out hit, footBoneDistanceToGround + maxFootElevation, walkableLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Vector3 footPosition = hit.point;
            footPosition.y += footBoneDistanceToGround;
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
        }
    }
}
