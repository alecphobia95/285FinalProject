using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerScript : MonoBehaviour
{
    public static PoolerScript instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolerDictionary = new Dictionary<string, Queue<GameObject>>();

    [HideInInspector]
    public GameObject objectToSpawn;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int x = 0; x < pool.size; x++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolerDictionary.Add(pool.tag, objectPool);
        }
    }

    public void SpawnFromPool(string tag, Vector3 position, Vector3 rotation)
    {
        if (!poolerDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist");
            return;
        }

        objectToSpawn = poolerDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = Quaternion.Euler(rotation);

        poolerDictionary[tag].Enqueue(objectToSpawn);
    }
}
