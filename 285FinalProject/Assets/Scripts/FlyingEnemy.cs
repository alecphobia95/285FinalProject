using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float roamRadius;
    public Vector2 originPoint;
    public float speed;

    public GameObject poolPrefab;
    public int poolSize;
    
    [HideInInspector]
    public GameObject objectToSpawn;
    
    private Queue<GameObject> _bulletPool;

    private Vector2 targetPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.position;
        targetPoint = originPoint + Random.insideUnitCircle * roamRadius;
        
        _bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolPrefab);
            obj.SetActive(false);
            _bulletPool.Enqueue(obj);
        }

        StartCoroutine(ShootBulletLoop());
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position =
            Vector2.MoveTowards(transform.position, targetPoint, step);
        if (Vector2.Distance(transform.position, targetPoint) < 0.001f)
        {
            targetPoint = originPoint + Random.insideUnitCircle * roamRadius;
        }
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

            float angleShot = Vector2.Angle(transform.position, targetPoint);
            float horizontalVel = speed * Mathf.Cos(angleShot);
            float verticallVel = speed * Mathf.Sin(angleShot);

            script.horiVel = horizontalVel;
            script.vertVel = verticallVel;

            yield return new WaitForSeconds(1f);
        }
    }
    
    public void Death()
    {
        Destroy(gameObject);
    }
}
