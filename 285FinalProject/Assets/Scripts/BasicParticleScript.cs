using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParticleScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float velocity;
    [HideInInspector]
    public float vertVel, horiVel;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horiVel, vertVel);
    }
}
