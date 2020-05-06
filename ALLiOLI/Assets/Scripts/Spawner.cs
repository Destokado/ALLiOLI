
using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Spawners have been created. Destroying the script of " + gameObject.name, gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject Spawn(GameObject prefab)
    {
        //TODO: Maybe it should be a pool?
        EasyRandom random = new EasyRandom();
        Vector3 spawnPoint = new Vector3(transform.position.x + random.GetRandomFloat(-2, 2), transform.position.y,
            transform.position.z + random.GetRandomFloat(-2, 2));
        return Instantiate(prefab, spawnPoint, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        var character = other.GetComponent<Character>();
        if(character && character.flag)
            MatchManager.Instance.MatchFinished(character.owner);
    }
}
