using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticlesManager : MonoBehaviour
{
    Pool particlePool;
    [SerializeField] private GameObject particleSystemPrefab;

    public static HitParticlesManager Instance;
    
    private void Awake()
    {
        Instance = this;
        particlePool = new Pool(particleSystemPrefab, 10, true);
    }

    public void DisplayAt(Vector3 position)
    {
        GameObject particles = particlePool.Spawn(position, Quaternion.identity, Vector3.one);
        particles.GetComponent<ParticleSystem>().Play(true); // Maybe needs to search in children
    }
}
