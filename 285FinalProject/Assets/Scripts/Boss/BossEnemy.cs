using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    IDLE,
    PATROL,
    DASH,
    JUMP
}

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public float speed;

    private int _currentPoint = 0;
    private int _nextPoint = 1;

    private BossState _state = BossState.IDLE;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
