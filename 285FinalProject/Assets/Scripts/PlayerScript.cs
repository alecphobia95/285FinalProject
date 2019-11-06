using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform[] attackSpawns;

    private int currentWep;
    public GameObject[] attackPrefabs;

    public float checkRadius;
    public float checkMookRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsMook;

    private bool control;
    private bool onGround, onMook;
    private bool rightWallPress, leftWallPress;
    private bool leftInput, rightInput, upInput, downInput, jumpInput, jumpHold, shootInput;
    private int airJumps;
    private string horiAim, vertAim, aim;
    
    private float gravity, halfGravity;

    // Use this for initialization
    void Start()
    {
        onGround = false;
        control = true;
        gravity = rb.gravityScale;
        halfGravity = rb.gravityScale * .5f;
        horiAim = "right";
        vertAim = "";
        currentWep = 0;
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
        SetPlayerAim();
        HandleShooting();
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            shootInput = true;
            //Debug.Log("Enter has been pressed");
        }

    }

    //This is just to be used for aiming ranged weapons for use in a future switch statement
    void SetPlayerAim()
    {
        aim = "left";
        if (upInput)
        {
            vertAim = "Up";
        }
        else if (downInput)
        {
            vertAim = "Down";
        }
        else
        {
            vertAim = "";
        }

        if(vertAim != "" && (!leftInput && !rightInput))
        {
            aim = vertAim;
        }
        else
        {
            aim = horiAim + vertAim;
        }
        //Debug.Log(aim);
    }

    void ClearInputs()
    {
        leftInput = false;
        rightInput = false;
        upInput = false;
        downInput = false;
        jumpInput = false;
        jumpHold = false;
        shootInput = false;
    }

    void HandleShooting()
    {
        if (shootInput)
        {
            GameObject bullet;
            BasicParticleScript script;
            Debug.Log(aim);
            switch (aim)
            {
                case "right":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[0]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = script.velocity;
                    script.vertVel = 0;
                    break;
                case "rightDown":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[1]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = script.velocity/2;
                    script.vertVel = -script.velocity/2;
                    break;
                case "Down":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[2]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = 0;
                    script.vertVel = -script.velocity;
                    break;
                case "leftDown":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[3]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = -script.velocity / 2;
                    script.vertVel = -script.velocity / 2;
                    break;
                case "left":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[4]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = -script.velocity;
                    script.vertVel = 0;
                    break;
                case "leftUp":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[5]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = -script.velocity / 2;
                    script.vertVel = script.velocity / 2;
                    break;
                case "Up":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[6]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = 0;
                    script.vertVel = script.velocity;
                    break;
                case "rightUp":
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[7]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = script.velocity / 2;
                    script.vertVel = script.velocity / 2;
                    break;
                default:
                    bullet = Instantiate(attackPrefabs[currentWep], attackSpawns[0]);
                    script = bullet.GetComponent<BasicParticleScript>();
                    script.horiVel = script.velocity;
                    script.vertVel = 0;
                    break;
            }
        }
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
        //if you wish to require input toward wall to walljump then add && rightInput
        if (jumpInput && !onGround && rightWallPress && !leftWallPress)
        {
            WallJump(-1);
        }
        //if you wish to require input toward wall to walljump then add && leftInput
        if (jumpInput && !onGround && !rightWallPress && leftWallPress)
        {
            WallJump(1);
        }
        if (control == true || onGround)
        {
            //Debug.Log("Good on walk");
            if (leftInput && !leftWallPress)
            {
                //Debug.Log("Initiating lateral movement");
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                horiAim = "left";
            }
            if (rightInput && !rightWallPress)
            {
                //Debug.Log("Initiating lateral movement");
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                horiAim = "right";
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

    private void WallJump(int direction)
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