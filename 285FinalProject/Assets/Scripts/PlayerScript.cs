using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed;
    public float jumpheight;
    public int maxJumps;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform rightCheck;
    public Transform leftCheck;
    public Transform enemyCheck;
    public float checkRadius;
    public float checkMookRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsMook;

    private bool control;
    private bool onGround, onMook;
    private bool rightWallPress, leftWallPress;
    private bool leftInput, rightInput, upInput, downInput, jumpInput, jumpHold;
    private int airJumps;
    
    private float gravity, halfGravity;

    // Use this for initialization
    void Start()
    {
        onGround = false;
        control = true;
        gravity = rb.gravityScale;
        halfGravity = rb.gravityScale * .5f;
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, whatIsGround);
        onMook = Physics2D.OverlapCircle(enemyCheck.transform.position, checkMookRadius, whatIsMook);
        rightWallPress = Physics2D.OverlapCircle(rightCheck.transform.position, checkRadius, whatIsGround);
        leftWallPress = Physics2D.OverlapCircle(leftCheck.transform.position, checkRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        GrabInputs();
        if (onGround)
        {
            airJumps = maxJumps;
        }
        RegularMovment();
        ClearInputs();
    }

    void GrabInputs()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            upInput = true;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftInput = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rightInput = true;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            downInput = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpHold = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }

    }

    void ClearInputs()
    {
        leftInput = false;
        rightInput = false;
        upInput = false;
        downInput = false;
        jumpInput = false;
        jumpHold = false;
    }

    //Just using this as a basis for very simple movement
    void RegularMovment()
    {
        if (jumpInput && onGround)
        {
            Jump();
        }
        if (jumpInput && !onGround && airJumps > 0 && !rightWallPress && !leftWallPress)
        {
            Jump();
            airJumps--;
            control = true;
        }
        if (jumpInput && !onGround && rightWallPress && !leftWallPress)
        {
            WallJump(-1, "left");
        }
        if (jumpInput && !onGround && !rightWallPress && leftWallPress)
        {
            WallJump(1, "right");
        }
        if (control == true || onGround)
        {
            //Debug.Log("Good on walk");
            if (leftInput && !leftWallPress)
            {
                Debug.Log("Initiating lateral movement");
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            if (rightInput && !rightWallPress)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            if (leftInput && leftWallPress)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (rightInput && rightWallPress)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if(leftInput && rightInput)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
        }
        if(rb.velocity.y > 0f && jumpHold)
        {
            rb.gravityScale = halfGravity;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpheight);
    }

    private void WallJump(int direction, string leftOrRight)
    {
        rb.velocity = new Vector2((moveSpeed * direction), jumpheight);
        control = false;
        Invoke("ReturnControl", 0.3f);
    }

    private void ReturnControl()
    {
        control = true;
    }

}