using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    private bool bomb = false;
    private Vector2 bombPoint;
    private float bombSpeed = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!bomb)
            return;
        float step = bombSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, bombPoint, step);
        if (Vector2.Distance(transform.position, bombPoint) < 0.001f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bomb)
            return;
        
        Collider2D overlapCollider = Physics2D.OverlapCircle(transform.position, 4, LayerMask.GetMask("Player"), 0);
        
        if (overlapCollider != null)
        {
            bomb = true;
            bombPoint = overlapCollider.gameObject.transform.position;
        }
    }
}
