using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndDamageScript : MonoBehaviour
{
    public float health;//assign in inspector for each item that uses this script
    public GameObject me;

    private bool invulnerable;

    private void Start()
    {
        invulnerable = false;
    }

    public void TakeDamage(float damage, float invulnPeriod)
    {
        if (!invulnerable)
        {
            health -= damage;
            invulnerable = true;

            if (me.CompareTag("Player"))
            {
                PlayerScript.instance.LifeText.text = "Life: " + health;

            }

            if (health <= 0f)
            {
                //put what you want on death to happen
                if(me.CompareTag("Player"))
                {  
                    PlayerScript script = me.GetComponent<PlayerScript>();
                    script.Death();
                }
                if (me.CompareTag("Mech"))
                {
                    MechScript script = me.GetComponent<MechScript>();
                    script.Death();
                }
                //put tags of other items here
                if (me.CompareTag("Patroller"))
                {
                    PatrolEnemy script = me.GetComponent<PatrolEnemy>();
                    script.Death();
                }
                if (me.CompareTag("Flyer"))
                {
                    FlyingEnemy script = me.GetComponent<FlyingEnemy>();
                    script.Death();
                }
                if (me.CompareTag("Bomber"))
                {
                    BomberEnemy script = me.GetComponent<BomberEnemy>();
                    script.Death();
                }
                if (me.CompareTag("Boss"))
                {
                    BossEnemy script = me.GetComponent<BossEnemy>();
                    script.Death();
                }
            }
            else
            {
                Invoke("RemoveInvuln", invulnPeriod);
            }
        }
    }

    private void RemoveInvuln()
    {
        invulnerable = false;
    }

}
