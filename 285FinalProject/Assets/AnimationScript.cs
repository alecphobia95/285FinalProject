using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public float walkspeed = 10f;
    public float runSpeed = 20.0f;
    public float jumpHeight;
    Animator animController;
    public Rigidbody2D playerRB2D;
    public GameObject player;


    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask thisIsGrounding;
    private bool grounded;

    private int doubleJump;
    private int maxJumps;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        maxJumps = 1;
        count = 0;
    }


    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, thisIsGrounding);

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            playerRB2D.velocity = new Vector2(walkspeed, playerRB2D.velocity.y);
            animController.SetBool("isWalking", true);
            Debug.Log("Moving");
            }
        }
    }

