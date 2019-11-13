using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    public GameObject self;

    public void HideMe()
    {
        Invoke("Hide", .1f);
    }

    void Hide()
    {
        self.SetActive(false);
    }

}
