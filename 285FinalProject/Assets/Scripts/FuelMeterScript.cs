using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelMeterScript : MonoBehaviour
{
    public static FuelMeterScript instance;

    public Slider fuelMeter;

    [HideInInspector]
    public float fuelStatus;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        fuelMeter.value = fuelStatus;
    }
}
