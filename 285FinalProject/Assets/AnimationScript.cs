using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpHeight;
    Animator animController;
    public Rigidbody2D playerRB2D;
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
    
    }


    private void FixedUpdate()
    {


    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            //going right
            animController.SetBool("isGoingRight", true);
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            //going left
            animController.SetBool("isGoingLeft", true);
            transform.position -= Vector3.right * speed * Time.deltaTime;  //vector3.left is the same as -=vector3.right
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            animController.SetBool("isGoingRight", false);
            animController.SetBool("isGoingLeft", false);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            animController.SetBool("isGoingUp", true);
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            animController.SetBool("isGoingDown", true);
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            animController.SetBool("isGoingDown", false);
            animController.SetBool("isGoingUp", false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            animController.SetBool("isPunching", true);
            print("get 'em, Boss!");
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            animController.SetBool("isPunching",false);
            print("Slow down, Boss!");
        }
    }
}

