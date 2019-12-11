using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float roamRadius;
    public Vector2 originPoint;
    public float speed;

    private Vector2 targetPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.position;
        targetPoint = originPoint + Random.insideUnitCircle * roamRadius;
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
}
