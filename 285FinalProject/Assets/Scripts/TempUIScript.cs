using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempUIScript : MonoBehaviour
{
    public static TempUIScript instance;

    public Text currentWepText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        ClearText();
    }

    public void CurrentWep(int whichWep)
    {
        string weapon = "";
        if(whichWep == 0)
        {
            weapon = "basic bullets";
        }
        if(whichWep == 1)
        {
            weapon = "melee swipe";
        }
        currentWepText.text = "Current weapon is " + weapon;

        Invoke("ClearText", 2f);
    }

    void ClearText()
    {
        currentWepText.text = "";
    }

}
