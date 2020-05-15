using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] flagPositions;
    [SerializeField] private GameObject flagPrefab;
    private EasyRandom rng;
    public static FlagSpawner Instance { get; private set; }
    public GameObject flag { get; private set; }

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
        }
    }

    private void Start()
    {
        rng = new EasyRandom();
    }

    public void Spawn()
    {
        if (flag != null) Destroy(flag);
        Vector3 spawnPos = flagPositions[rng.GetRandomInt(0, flagPositions.Length)].position;
        flag = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
        Debug.Log("The flag has spawn at:" + spawnPos);
    }
}