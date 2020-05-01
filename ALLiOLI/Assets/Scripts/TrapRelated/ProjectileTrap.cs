using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleAnimationsManager))]
public class ProjectileTrap : Trap
{
    
    private SimpleAnimationsManager animManager;
    [SerializeField] private List<Transform> projectilePos;
    [SerializeField] private GameObject projectilePrefab;
    private void Awake()
    {
        animManager = gameObject.GetComponent<SimpleAnimationsManager>();
    }

    protected override void Reload()
    {
        animManager.GetAnimation(0).mirror = true;
        animManager.Play(0);
    }

    public override void Activate()
    { 
        base.Activate();
        animManager.GetAnimation(0).mirror = false;
        animManager.Play(0);
        foreach (Transform pos in projectilePos)
        {
            GameObject projectile = Instantiate(projectilePrefab,pos.position,Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(Vector3.forward);
        }
    }
    
}