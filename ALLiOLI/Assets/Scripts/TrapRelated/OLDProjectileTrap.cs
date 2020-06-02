using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkPool))]
public class OLDProjectileTrap : Trap
{
    [Header("Explosion")] [SerializeField] private float explosionForce;
    [SerializeField] private Transform explosionPos;
    [SerializeField] private float explosionRadius;

    [Header("Projectile")] [SerializeField]
    private int projectileNumber;

    [SerializeField] private Vector3 minSpawnRadius;
    [SerializeField] private Vector3 maxSpawnRadius;

    private NetworkPool pool;
    private EasyRandom random;

    private void Awake()
    {
        pool = gameObject.GetComponentRequired<NetworkPool>();
        random = new EasyRandom();
    }

    protected override void Reload()
    {
    }

    public override void Activate()
    {
        base.Activate();

        for (int i = 0; i < projectileNumber; i++)
        {
            GameObject projectile = pool.Spawn();
            projectile.transform.position = GetRandomPositionInRadius(pool.instantiationTransform.position, minSpawnRadius, maxSpawnRadius);
            StartCoroutine(AddForce(projectile));
        }
    }


    private Vector3 GetRandomPositionInRadius(Vector3 center, Vector3 minRadius, Vector3 maxRadius)
    {
        Vector3 position = new Vector3(center.x + random.GetRandomFloat(minRadius.x, maxRadius.x),
            center.y + random.GetRandomFloat(minRadius.y, maxRadius.y),
            center.z + random.GetRandomFloat(minRadius.z, maxRadius.z));
        return position;
    }

    private IEnumerator AddForce(GameObject projectile)
    {
        yield return new WaitForFixedUpdate();
        projectile.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPos.position, explosionRadius,
            0.03f, ForceMode.Impulse);
    }
}