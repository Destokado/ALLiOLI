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
        SetFootPosition(AvatarIKGoal.LeftFoot);
        SetFootPosition(AvatarIKGoal.RightFoot);
    }

    private void SetFootPosition(AvatarIKGoal foot)
    {
        anim.SetIKPositionWeight(foot, 1f);
        anim.SetIKRotationWeight(foot, 1f);
        
        RaycastHit hit;
        Ray ray = new Ray(anim.GetIKPosition(foot)+Vector3.up*maxFootElevation, Vector3.down);
        if (Physics.Raycast(ray, out hit, Vector3.Distance(anim.GetIKPosition(foot), Vector3.up*maxFootElevation), walkableLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Vector3 footPosition = hit.point;
            footPosition.y += footBoneDistanceToGround;
            anim.SetIKPosition(foot, footPosition);
            anim.SetIKRotation(foot,Quaternion.LookRotation(transform.forward, hit.normal));
        } else {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
        }
    }
}
