using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingTubes : Trap
{
    
    [SerializeField] private int numberOfTubes = 2;
    [SerializeField] private float timeBetweenTubeSpanws = 0.5f;
    [SerializeField] private float tubeSpeed = 5f;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject tubePrefab;
    private Pool pool;
    
    
    private void Awake()
    {
        pool = new Pool(tubePrefab, numberOfTubes, spawnPos.position, this.transform.rotation, false);
    }

    protected override void Reload() { }

    public override void Activate()
    {
        base.Activate();
    
        StartCoroutine(SpawnTubes());
    }

    private IEnumerator SpawnTubes()
    {
        for (int t = 0; t < numberOfTubes; t++)
        {
            GameObject projectile = pool.Spawn(spawnPos.position, this.transform.rotation, Vector3.one);
            projectile.GetComponent<Rigidbody>().velocity = (Vector3.down)*tubeSpeed;
            yield return new WaitForSeconds(timeBetweenTubeSpanws);
        }
    }
}
