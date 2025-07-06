using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //Singleton pattern
    public static ObjectPooler i;

    //Inner class to define settings for each type of pool
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab; //Pooled object prefab
        public int size;
        public bool canExpand = true;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        // Implement Singleton pattern
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        InitializePools();
    }

    void InitializePools()
    {
        //Create dictionary and parent object to add contaners to it
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        GameObject poolsParent = new GameObject("PoolsParent");
        poolsParent.transform.SetParent(this.transform);

        foreach (Pool pool in pools)
        {
            //Create object pool Q and create object container for poolable objects
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject poolContainer = new GameObject(pool.tag + "Pool");
            poolContainer.transform.SetParent(poolsParent.transform);

            //Fill each pool with objects
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(poolContainer.transform);

                if (obj.TryGetComponent<IPoolable>(out IPoolable poolableComponent))
                {
                    poolableComponent.PoolTag = pool.tag;
                    poolableComponent.OnObjectDespawn();
                }
                else obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            //Add object pool Q to dictionary.
            poolDictionary.Add(pool.tag, objectPool);
            //Debug.Log($"Initialized pool '{pool.tag}' with {pool.size} objects.");
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        //Check if the pool tag exists
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];
        GameObject objectToSpawn = null;

        //If pool is empty
        if (objectPool.Count == 0)
        {
            Pool poolSettings = pools.Find(p => p.tag == tag);
            if (poolSettings != null && !poolSettings.canExpand)
            {
                Debug.LogWarning($"Pool '{tag}' is empty and cannot expand. Cannot spawn more objects.");
                return null;
            }

            Debug.LogWarning($"Pool '{tag}' is empty, instantiating new object (expanding pool).");
            objectToSpawn = Instantiate(poolSettings.prefab);

            if (objectToSpawn.TryGetComponent<IPoolable>(out IPoolable poolableComponent))
                poolableComponent.PoolTag = tag;
        }
        else
        {
            objectToSpawn = objectPool.Dequeue();
        }

        //Set position and rotation
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        //Activate object
        if (objectToSpawn.TryGetComponent<IPoolable>(out IPoolable poolable))
            poolable.OnObjectSpawn();
        else
            objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        //If no tag, just destroy
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist. Destroying object instead of pooling.");
            Destroy(objectToReturn);
            return;
        }

        //Deactivate
        if (objectToReturn.TryGetComponent<IPoolable>(out IPoolable poolable))
            poolable.OnObjectDespawn();
        else
            objectToReturn.SetActive(false);

        //Parent back to the pool container or root
        Transform poolContainer = transform.Find($"PoolsParent/{tag}Pool");
        if (poolContainer != null)
        {
            objectToReturn.transform.SetParent(poolContainer);
        }
        else
        {
            Debug.LogWarning($"Could not find pool container for tag '{tag}'. Parenting to ObjectPooler root.");
            objectToReturn.transform.SetParent(this.transform);
        }

        //Add back to the queue
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    void OnDestroy()
    {
        if (i == this) i = null;

        //Destroy all pooled objects if they should not persist, but only if DontDestroyOnLoad was not used
        /*foreach (var entry in poolDictionary)
        {
            while (entry.Value.Count > 0)
            {
                GameObject obj = entry.Value.Dequeue();
                if (obj != null) Destroy(obj);
            }
        }
        poolDictionary.Clear();*/
    }
}
