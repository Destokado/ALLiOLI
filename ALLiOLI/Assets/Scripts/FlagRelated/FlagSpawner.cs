using System.Collections;
using Boo.Lang;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FlagSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject flagPrefab;
    [SerializeField] private Transform spawnCenter;

    [SerializeField] private Vector2 spawnSize = Vector2.one;

    /* public Flag flag { get; private set; }
     private readonly EasyRandom rng = new EasyRandom();*/
    private List<Flag> flags;

    public static FlagSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple FlagSpawners have been created. Destroying the script of " + gameObject.name,
                gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
            flags = new List<Flag>();
        }
    }

    [Server]
    public Flag SpawnFlag(uint playerOwnerNetId, NetworkConnection playerOwnerConnectionToClient)
    {
        var spawnPoint = GetSpawnPos();

        GameObject flag = Instantiate(flagPrefab, spawnPoint, spawnCenter.rotation);
        Flag script = flag.GetComponent<Flag>();
        flags.Add(script);
        script.OwnerNetId = playerOwnerNetId;
        NetworkServer.Spawn(flag, playerOwnerConnectionToClient);
        //flag.SetActive(false);
        return script;
    }

    public Vector3 GetSpawnPos()
    {
        float deltaX = Random.Range(-spawnSize.x / 2f, spawnSize.x / 2f);
        float deltaY = Random.Range(-spawnSize.y / 2f, spawnSize.y / 2f);

        Vector3 centerPos = spawnCenter.position;
        Vector3 spawnPoint = new Vector3(centerPos.x + deltaX, centerPos.y, centerPos.z + deltaY);
        return spawnPoint;
    }

/*
    private void SpawnFlagOnAllClients(Vector3 spawnPos)
    {
        SpawnFlagInServer(spawnPos);
        flag.gameObject.SetActive(true);
        NetworkServer.Spawn(flag.gameObject);
    }

    [Server]
    private void SpawnFlagInServer(Vector3 spawnPos)
    {
        if (flag != null)
        {
            flag.transform.position = spawnPos;
            return;
        }
        GameObject flagGameObject = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
        flag = flagGameObject.GetComponentRequired<Flag>();
    }

    [Server]
    public void FlagPickedBy(Character character)
    {
        if (flag == null)
            Debug.LogError($"{character.gameObject.name} piked an non-instantiated flag.", character);

        NetworkServer.UnSpawn(flag.gameObject);
        flag.gameObject.SetActive(false);
        flag.AttachTo(character);
        Debug.Log($"Flag picked by: {character.gameObject.name}");
}
    
    [Server]
    public void FlagDropped(Vector3 position)
    {
        SpawnFlagOnAllClients(position);
        flag.Detach();
        Debug.Log($"The flag has been dropped at: {position}", flag);
    }

    [Server]
    public void ClearFlags()
    {
        if (!flag) return;
        
        flag.Reset();
        NetworkServer.UnSpawn(flag.gameObject);
        flag.gameObject.SetActive(false);
    }
    */
    /* public void ActivateFlags()
     {
         foreach (Flag flag in flags)
         {
             flag.gameObject.SetActive(true);
         }
         
     }*/
    [Server]
    public void ResetFlags()
    {
       RpcResetFlags();
    }

    [ClientRpc]
    private void RpcResetFlags()
    {
      
        foreach (Player p in   Client.LocalClient.PlayersManager.players)
        {
            p.Flag.Reset();
        }
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