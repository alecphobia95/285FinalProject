using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : MonoBehaviour
{
    private Queue<GameObject> _bulletPool;

    public GameObject poolPrefab;
    public int poolSize;
    
    [HideInInspector]
    public GameObject objectToSpawn;

    private Dictionary<string, float> _rotDictionary;
    
    // Start is called before the first frame update
    void Start()
    {
        _rotDictionary = new Dictionary<string, float>();
        _rotDictionary.Add("UP", 0);
        _rotDictionary.Add("LEFT", 90);
        _rotDictionary.Add("DOWN", 180);
        _rotDictionary.Add("RIGHT", 270);
        
        _bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolPrefab);
            obj.SetActive(false);
            _bulletPool.Enqueue(obj);
        }

        StartCoroutine(ShootBulletLoop());
    }
    
    public void SpawnFromPool(Vector3 position, Vector3 rotation)
    {
        objectToSpawn = _bulletPool.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = Quaternion.Euler(rotation);

        _bulletPool.Enqueue(objectToSpawn);
    }

    IEnumerator ShootBulletLoop()
    {
        while (true)
        {
            SpawnFromPool(transform.position, transform.eulerAngles);

            BasicParticleScript script = objectToSpawn.GetComponent<BasicParticleScript>();

            if (transform.eulerAngles.z == _rotDictionary["UP"] || transform.eulerAngles.z == _rotDictionary["DOWN"])
                script.vertVel = (transform.eulerAngles.z == _rotDictionary["UP"]) ? script.velocity : -script.velocity;
            else
                script.horiVel = (transform.eulerAngles.z == _rotDictionary["RIGHT"]) ? script.velocity : -script.velocity;

            yield return new WaitForSeconds(0.7f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
