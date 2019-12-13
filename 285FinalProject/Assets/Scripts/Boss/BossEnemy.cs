using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public enum BossState
{
    IDLE,
    PATROL,
    DASH
}

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> patrolPoints;
    public Animator bossAnimator;
    public float speed;

    private int _currentPoint = 0;
    private int _nextPoint = 1;
    private float _jumpDistance;

    private Dictionary<BossState, IEnumerator> _stateBehaviors;

    private BossState _state = BossState.IDLE;
    private Coroutine _currentBehavior;
    
    private Vector2 _dashTimeRange = new Vector2(7, 15);
    private float _dashTime = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentBehavior = StartCoroutine(IdleBehavior());
        
        _stateBehaviors = new Dictionary<BossState, IEnumerator>();

        _stateBehaviors[BossState.PATROL] = PatrolBehaviorEnumerator();
        _stateBehaviors[BossState.IDLE] = IdleBehavior();
        _stateBehaviors[BossState.DASH] = DashBehavior();
    }

    // Update is called once per frame
    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.B))
//            StateSwitcher(BossState.PATROL);
//        if (Input.GetKeyDown(KeyCode.N))
//            StateSwitcher(BossState.IDLE);
//        if (Input.GetKeyDown(KeyCode.V))
//            StateSwitcher(BossState.DASH);

        if (Time.time >= _dashTime && _state != BossState.DASH && _dashTime != -1)
        {
            StateSwitcher(BossState.DASH);
        }
        
    }

    private void FixedUpdate()
    {
        if (_state != BossState.IDLE)
            return;
        Collider2D overlapCollider = Physics2D.OverlapBox(transform.position, new Vector2(20, 10), 0, LayerMask.GetMask("Player"));
        
        if (overlapCollider != null)
        {
            _dashTime = Time.time + Random.Range(_dashTimeRange.x, _dashTimeRange.y);
            StateSwitcher(BossState.PATROL);
        }
    }

    IEnumerator DashBehavior()
    {
        yield return new WaitForSeconds(2.0f);
        float originalSpeed = speed;
        speed = originalSpeed * 3;
        float dashStartTime = Time.time;
        
        while (Time.time < dashStartTime + 5.0f)
        {
            PatrolBehavior();
            
            yield return null;
        }

        speed = originalSpeed;
        _dashTime = Time.time + Random.Range(_dashTimeRange.x, _dashTimeRange.y);
        
        StateSwitcher(BossState.PATROL);
    }
    
    IEnumerator IdleBehavior()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void PatrolBehavior()
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

    IEnumerator PatrolBehaviorEnumerator()
    {
        while (true)
        {
            PatrolBehavior();

            yield return null;
        }
        
    }

    private void StateSwitcher(BossState state)
    {
        if (state == _state)
            return;
        _state = state;
        
        IEnumerator nextBehavior = _stateBehaviors[state];
        
        StopCoroutine(_currentBehavior);
        if (_state == BossState.DASH)
            _currentBehavior = StartCoroutine("DashBehavior");
        else
            _currentBehavior = StartCoroutine(nextBehavior);
        
        if (!bossAnimator)
            return;
        
        switch (state)
        {
            case BossState.IDLE:
                bossAnimator.SetTrigger("Idle");
                break;
            case BossState.PATROL:
                bossAnimator.SetTrigger("Walk");
                break;
            case BossState.DASH:
                bossAnimator.SetTrigger("Walk");
                break;
        }
    }
    
    public void Death()
    {
        Destroy(gameObject);
    }
}
