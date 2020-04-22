using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public static TrapManager Instance { get; private set; }
    private LinkedList<ATrap> traps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ATrap GetClosestTrap(Transform transform1)
    {
        throw new System.NotImplementedException();
    }
}
