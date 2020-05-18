using System.Collections;
using UnityEngine;

public class RollingTubes : Trap
{
    [SerializeField] private int numberOfTubes = 2;
    private Pool pool;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float timeBetweenTubeSpanws = 0.5f;
    [SerializeField] private GameObject tubePrefab;
    [SerializeField] private float tubeSpeed = 5f;


    private void Awake()
    {
        pool = new Pool(tubePrefab, numberOfTubes, spawnPos.position, transform.rotation);
    }

    protected override void Reload()
    {
    }

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(SpawnTubes());
    }

    private IEnumerator SpawnTubes()
    {
        for (int t = 0; t < numberOfTubes; t++)
        {
            GameObject projectile = pool.Spawn(spawnPos.position, transform.rotation, Vector3.one);
            projectile.GetComponent<Rigidbody>().velocity = Vector3.down * tubeSpeed;
            yield return new WaitForSeconds(timeBetweenTubeSpanws);
        }
    }
}