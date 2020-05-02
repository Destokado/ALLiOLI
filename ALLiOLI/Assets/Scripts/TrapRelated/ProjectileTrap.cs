using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleAnimationsManager))]
public class ProjectileTrap : Trap
{

    [SerializeField] private List<Transform> projectilePos;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private Transform explosionPos;

    protected override void Reload()
    {
       /* animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);*/
    }

    public override void Activate()
    {
        base.Activate();
       
       foreach (Transform trans in projectilePos)
       {
           GameObject projectile = Instantiate(projectilePrefab, trans.position, Quaternion.identity);
           projectile.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPos.position, explosionRadius,0.1f, ForceMode.Impulse);
           Destroy(projectile,10f);
       }
            
    }
}