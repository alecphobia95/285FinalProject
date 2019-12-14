using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParticleScript : MonoBehaviour
{
    public GameObject self;
    public Rigidbody2D rb;

    public float velocity;
    [HideInInspector]
    public float vertVel, horiVel;

    void Update()
    {
        rb.velocity = new Vector2(horiVel, vertVel);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            self.SetActive(false);
        }
        if(collision.gameObject.layer == 13 && gameObject.layer == 10)
        {
            collision.gameObject.GetComponent<HealthAndDamageScript>().TakeDamage(10, 0);
            self.SetActive(false);
        }
        
        if(collision.gameObject.tag == "Player")
        {
            if (self.CompareTag("EnemyBullet"))
            {
                collision.gameObject.GetComponent<HealthAndDamageScript>().TakeDamage(5, 0);
                self.SetActive(false);
            }
            
            if (self.tag == "MechBullet")
            {
                self.SetActive(false);
            }
        }
        if (collision.gameObject.tag == "Mech")
        {
            if (self.tag == "BasicBullet")
            {
                self.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 13 && gameObject.layer == 10)
        {
            other.gameObject.GetComponent<HealthAndDamageScript>().TakeDamage(10, 0);
            self.SetActive(false);
        }
    }
}
