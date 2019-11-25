﻿using System.Collections;
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
        if(collision.gameObject.tag == "Player")
        {
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

}
