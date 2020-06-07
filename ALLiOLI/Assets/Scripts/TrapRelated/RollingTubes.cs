using System.Collections;
using Mirror;
using UnityEngine;

public class RollingTubes : Trap
{
    private NetworkPool pool;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private float timeBetweenTubeSpanws = 0.5f;
    [SerializeField] private GameObject tubePrefab;
    [SerializeField] private float tubeSpeed = 5f;
    
    private void Awake()
    {
        pool = gameObject.GetComponentRequired<NetworkPool>();
    }

    protected override void Reload()
    {
        base.Reload();
    }

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(SpawnTubes());
    }

    private IEnumerator SpawnTubes()
    {
            for (int t = 0; t < pool.size; t++)
            {
                pool.instantiationTransform = spawnPos[Random.Range(0,spawnPos.Length-1)];
                
                GameObject projectile = pool.Spawn();
                
                projectile.GetComponent<Rigidbody>().velocity = Vector3.down * tubeSpeed;
                projectile.GetComponent<KillZone>().trap = this;
                yield return new WaitForSeconds(timeBetweenTubeSpanws);
            }
    }
}