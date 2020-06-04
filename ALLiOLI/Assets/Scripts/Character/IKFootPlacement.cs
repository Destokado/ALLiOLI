using System;
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
        Ray ray = new Ray(anim.GetIKPosition(foot) + Vector3.up * maxFootElevation, Vector3.down);
        float distanceFromMaxElevation =
            Vector3.Distance(anim.GetIKPosition(foot) + Vector3.down * footBoneDistanceToGround, ray.origin);
        
        if (Physics.Raycast(ray, out hit, distanceFromMaxElevation, walkableLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            Vector3 footPosition = hit.point;
            footPosition.y += footBoneDistanceToGround;
            anim.SetIKPosition(foot, footPosition);
            anim.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin+ray.direction*distanceFromMaxElevation, Color.green);
        }
    }

    /*private void OnDrawGizmosSelected() 
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawSphere(anim.GetIKPosition(AvatarIKGoal.LeftFoot), 0.05f); // Warning
        Gizmos.DrawSphere(anim.GetIKPosition(AvatarIKGoal.RightFoot), 0.05f); // Warning
    }*/
}