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

    protected override void Awake()
    {
        base.Awake();
        pool = gameObject.GetComponentRequired<NetworkPool>();
    }

    protected override void Reload()
    {
        base.Reload();

        //pool.DisableAllInstanciated();
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
                //projectile.GetComponent<TrapKillZone>().trap = this;
                projectile.GetComponent<NetworkTrapKillZone>().trapNetId = this.netId;
                yield return new WaitForSeconds(timeBetweenTubeSpanws);
            }
    }
}