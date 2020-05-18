using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkPool))]
public class ProjectileTrap : Trap
{
    [SerializeField] private float explosionForce;
    [SerializeField] private Transform explosionPos;
    [SerializeField] private float explosionRadius;
    private NetworkPool pool;

    [SerializeField] private List<Transform> projectilePos;

    private void Awake()
    {
        pool = gameObject.GetComponentRequired<NetworkPool>();
    }

    protected override void Reload()
    {
    }

    public override void Activate()
    {
        base.Activate();

        foreach (Transform trans in projectilePos)
        {
            GameObject projectile = pool.Spawn();
            StartCoroutine(AddForce(projectile));
        }
    }

    private IEnumerator AddForce(GameObject projectile)
    {
        yield return new WaitForFixedUpdate();
        projectile.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPos.position, explosionRadius,
            0.03f, ForceMode.Impulse);
    }
}