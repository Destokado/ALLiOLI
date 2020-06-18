using System;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Spawner : NetworkBehaviour
{
    public static Spawner Instance { get; private set; }
    [SerializeField] private Transform spawnCenterAndRotation;
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
    public void SpawnCharacter(GameObject prefab, uint playerOwnerNetId, NetworkConnection playerOwnerConnectionToClient)
    {
        Vector3 spawnPoint = GetSpawnPos();

        GameObject characterGo = Instantiate(prefab, spawnPoint, spawnCenterAndRotation.rotation);
        characterGo.GetComponent<Character>().OwnerNetId = playerOwnerNetId;
        NetworkServer.Spawn(characterGo, playerOwnerConnectionToClient);
    }

    public Vector3 GetSpawnPos()
    {
        float deltaX = UnityEngine.Random.Range(-spawnSize.x / 2f, spawnSize.x / 2f);
        float deltaY = UnityEngine.Random.Range(-spawnSize.y / 2f, spawnSize.y / 2f);

        Vector3 centerPos = spawnCenterAndRotation.position;
        Vector3 spawnPoint = new Vector3(centerPos.x + deltaX, centerPos.y, centerPos.z + deltaY);
        return spawnPoint;
    }

    public Quaternion GetSpawnRotation()
    {
        return spawnCenterAndRotation.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(MatchManager.instance.thereIsWinner||!isServer) return;
        
        Character character = other.GetComponentInParent<Character>();
        if (character && character.HasFlag)
            MatchManager.instance.FlagAtSpawn(character.Owner);

        else
        {
            Flag flag = other.GetComponent<Flag>();
            
            if (flag)
                MatchManager.instance.FlagAtSpawn(flag.Owner);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.zTest = CompareFunction.LessEqual;
        Handles.color = Color.cyan;
        Handles.DrawWireCube(spawnCenterAndRotation.position, new Vector3(spawnSize.x, 0, spawnSize.y));
    }
    #endif

}