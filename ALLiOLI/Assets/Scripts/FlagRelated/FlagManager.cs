using Mirror;
using UnityEngine;

public class FlagManager : NetworkBehaviour
{
    [SerializeField] private Transform[] flagPositions;
    [SerializeField] private GameObject flagPrefab;
    
    public Flag flag { get; private set; }
    private readonly EasyRandom rng = new EasyRandom();

    public static FlagManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning("Multiple FlagSpawners have been created. Destroying the script of " + gameObject.name,
                gameObject);
            Destroy(this);
        } else { Instance = this; }
    }

    [Server]
    public void Spawn()
    {
        Vector3 spawnPos = flagPositions[rng.GetRandomInt(0, flagPositions.Length)].position;
        SpawnFlagOnAllClients(spawnPos);
        Debug.Log($"The flag has been spawned at: {spawnPos}", flag);
    }

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
        character.hasFlag = true;
        Debug.Log($"Flag picked by: {character.gameObject.name}");
}
    
    [Server]
    public void FlagDropped(Vector3 position)
    {
        SpawnFlagOnAllClients(position);
        flag.carrier.hasFlag = false;
        flag.Detach();
        Debug.Log($"The flag has been dropped at: {position}", flag);
    }
}