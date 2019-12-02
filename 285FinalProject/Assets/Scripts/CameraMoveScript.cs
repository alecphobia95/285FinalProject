using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public Transform thisTransform;
    public float bounds;

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 playerPos = thisTransform.transform.position;

        if(camPos.x < playerPos.x - bounds)
        {
            camPos.x = playerPos.x - bounds;
        }
        else if (camPos.x > playerPos.x + bounds)
        {
            camPos.x = playerPos.x + bounds;
        }
        if (camPos.y < playerPos.y - bounds)
        {
            camPos.y = playerPos.y - bounds;
        }
        else if (camPos.y > playerPos.y + bounds)
        {
            camPos.y = playerPos.y + bounds;
        }

        camPos.z = -100;
        Camera.main.transform.position = camPos;
    }
}
