using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    public static Spawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Spawns have been created. Destroying the script of " + gameObject.name,
                gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /*public void Spawn(GameObject prefab, uint netId)
    {
        CmdSpawn(prefab, netId);
    }*/


    public void Spawn(GameObject prefab, uint playerOwnerNetId, NetworkConnection playerOwnerConnectionToClient)
    {
        EasyRandom random = new EasyRandom();
        Vector3 spawnPoint = new Vector3(transform.position.x + random.GetRandomFloat(-2, 2), transform.position.y,
            transform.position.z + random.GetRandomFloat(-2, 2));

        GameObject character = Instantiate(prefab, spawnPoint, Quaternion.identity);
        character.GetComponent<Character>().PlayerSpawnerNetId = playerOwnerNetId;
        NetworkServer.Spawn(character, playerOwnerConnectionToClient);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character && character.flag)
            return; // TODO: finish match
        //MatchManager.Instance.MatchFinished(character.owner);
    }
}