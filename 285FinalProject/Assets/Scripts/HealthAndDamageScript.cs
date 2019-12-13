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

            if (health <= 0f)
            {
                //put what you want on death to happen
                if(me.tag == "Player")
                {
                    PlayerScript script = me.GetComponent<PlayerScript>();
                    script.Death();
                }
                if (me.tag == "Mech")
                {
                    MechScript script = me.GetComponent<MechScript>();
                    script.Death();
                }
                //put tags of other items here
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
