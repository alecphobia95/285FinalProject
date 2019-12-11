using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public float speed;

    private int _currentPoint = 0;
    private int _nextPoint = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position =
            Vector2.MoveTowards(transform.position, patrolPoints[_currentPoint].transform.position, step);
        
        if (Vector2.Distance(transform.position, patrolPoints[_currentPoint].transform.position) < 0.001f)
        {
            // Swap the patrol points
            int tempPoint = _currentPoint;
            _currentPoint = _nextPoint;
            _nextPoint = tempPoint;
        }
    }
}
