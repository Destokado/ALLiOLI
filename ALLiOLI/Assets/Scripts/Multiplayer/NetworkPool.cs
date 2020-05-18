using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Mirror
{
    /// <summary>
    /// Imitates the spawning system of the class "Pool" in the asset "Essentials" but adapted to be properly working in a network if being used by the server.
    /// </summary>
    public class NetworkPool : NetworkBehaviour
    {
        /// <summary>
        /// The object that will be spawned by the pool.
        /// </summary>
        [SerializeField] public GameObject baseObject;
        /// <summary>
        /// The transform that the spawned objects will clone every time they are spawned.
        /// </summary>
        [Tooltip("The transform that the spawned objects will clone every time they are spawned.")]
        [SerializeField] public Transform instantiationTransform;
        /// <summary>
        /// The maximum number of objects that can be instantiated at the same time.
        /// </summary>
        [Tooltip("The maximum number of objects that can be instantiated at the same time.")]
        [SerializeField] public int size = 1;
        /// <summary>
        /// If the pool should spawn all the objects in the scene right away (true) or if they should be instantiated when they are needed (false).
        /// </summary>
        [Tooltip("If the pool should spawn all the objects in the scene right away (true) or if they should be instantiated when they are needed (false).")]
        [SerializeField] public bool spawnOnStart = false;
        
        private List<GameObject> referencedObjects = new List<GameObject>();
        private int activeIndex = 0;
        
        
        public override void OnStartServer()
        {
            if (!spawnOnStart) return;
            
            for (int i = 0; i < size; i++)
                InstantiateNewAt(i);
        }

        /// <summary>
        /// Activates an object from the pool.
        /// <para>The activated object will be chosen dynamically looping between all the objects in the pool.</para>
        /// </summary>
        [Server]
        public GameObject Spawn()
        {
            if (activeIndex >= referencedObjects.Count || referencedObjects[activeIndex] == null)
                InstantiateNewAt(activeIndex);

            referencedObjects[activeIndex].transform.SetProperties(instantiationTransform);
            referencedObjects[activeIndex].SetActive(true);

            GameObject goToReturn = referencedObjects[activeIndex];

            activeIndex = activeIndex.GetLooped(size); //index = index+1<referencedObjects.Length? index+1 : 0;

            return goToReturn;
        }

        //Only instantiates new elements if the pool size is preserved and if there are non existing GameObjects in the indicated index.
        [Server]
        private GameObject InstantiateNewAt(int index)
        {
            if (referencedObjects.Count >= size ||
                (index < referencedObjects.Count && referencedObjects[index] != null))
                return null;

            GameObject go = Instantiate(baseObject, instantiationTransform.position, instantiationTransform.rotation);
            NetworkServer.Spawn(go);
            go.SetActive(false);
            referencedObjects.Add(go);
            return go;
        }
        
    }
}