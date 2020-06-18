using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mirror;
using UnityEngine;

[SelectionBase]
public class TrapActivator : NetworkBehaviour
{
    [SyncVar]
    private bool activated = true;

    private void Awake()
    {
        myMeshes = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        
        MeshRenderer m = GetComponent<MeshRenderer>();
        if (m != null && !myMeshes.Contains(m)) 
            myMeshes.Add(m);
    }

    private float remainingCoolDown = 0f;
    [SerializeField] private float coolDownTime = 5f;

    private List<MeshRenderer>myMeshes = new List<MeshRenderer>();
    private float dissolvePercent; 
    private float dissolveDuration = 3f;
    private float appearingDuration = 1f;
    private static readonly int DissolvePercent = Shader.PropertyToID("DISSOLVE_PERCENT");
    private void Update()
    {
        if (activated)
        {
            if (dissolvePercent > 0)
            {
                dissolvePercent -= appearingDuration*Time.deltaTime;
            }
        }

        if (!activated)
        {
            if (dissolvePercent < 1)
            {
                dissolvePercent += dissolveDuration*Time.deltaTime;
            }
        }

        dissolvePercent = Mathf.Clamp01(dissolvePercent);
         
        MaterialPropertyBlock bundle = new MaterialPropertyBlock();
        bundle.SetFloat( DissolvePercent, dissolvePercent);
        foreach (MeshRenderer mr in myMeshes)
            mr.SetPropertyBlock(bundle);
        
        
        
        if (!isServer || (remainingCoolDown <= 0)) return;

        remainingCoolDown -= Time.deltaTime;
        if (remainingCoolDown <= 0)
            activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated || !isServer || remainingCoolDown > 0) return;

        Character character = other.GetComponentInParent<Character>();
        if (character)
        {
            SoundManager.Instance.PlayOneShotLocal(SoundManager.EventPaths.PickActivator, transform.position, null);
            activated = false;
            remainingCoolDown = coolDownTime;
            character.Owner.trapActivators += 1;
        }
    }
}