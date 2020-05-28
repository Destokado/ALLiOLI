using System;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Spawner : NetworkBehaviour
{
    public static Spawner Instance { get; private set; }
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private Vector2 spawnSize = Vector2.one;
    

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


    [Server]
    public void Spawn(GameObject prefab, uint playerOwnerNetId, NetworkConnection playerOwnerConnectionToClient)
    {
        float deltaX = UnityEngine.Random.Range(-spawnSize.x / 2f, spawnSize.x / 2f);
        float deltaY = UnityEngine.Random.Range(-spawnSize.y / 2f, spawnSize.y / 2f);

        Vector3 centerPos = spawnCenter.position;
        Vector3 spawnPoint = new Vector3(centerPos.x + deltaX, centerPos.y, centerPos.z + deltaY);

        GameObject character = Instantiate(prefab, spawnPoint, spawnCenter.rotation);
        character.GetComponent<Character>().OwnerNetId = playerOwnerNetId;
        NetworkServer.Spawn(character, playerOwnerConnectionToClient);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer)
            return;
        
        Character character = other.GetComponent<Character>();
        if (character && character.flag)
            MatchManager.Instance.FlagAtSpawn(character.Owner);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.zTest = CompareFunction.LessEqual;
        Handles.color = Color.cyan;
        Handles.DrawWireCube(spawnCenter.position, new Vector3(spawnSize.x, 0, spawnSize.y));
    }
    #endif

}