using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{


    public float moveSpeed;
    public float jumpStrength;
    public int maxJumps;
    public float gravity, reducedGravity;
    public Rigidbody2D rb;
    Animator animController;

    public Transform groundCheck;
    public Transform[] rightCheck;
    public Transform[] leftCheck;
    public Transform enemyCheck;
    public float checkRadius;
    public float checkMookRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsMook;

    public Transform[] attackSpawns;
    public GameObject[] attackPrefabs;
    public float[] attackCooldowns;
    private bool[] canShoot;
    private int currentWep;

    private bool control;
    private bool onGround, onMook;
    private bool rightWallPress, leftWallPress;
    private bool leftInput, rightInput, upInput, downInput, jumpInput, jumpHold, shootInput;
    private int airJumps;
    private string horiAim, vertAim, aim;

    // Use this for initialization
    void Start()
    {
        onGround = false;
        control = true;
        horiAim = "right";
        vertAim = "";
        currentWep = 0;
        rb.gravityScale = gravity;
        SetUpArrays();
        ResetCooldowns();

    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, whatIsGround);
        onMook = Physics2D.OverlapCircle(enemyCheck.transform.position, checkMookRadius, whatIsMook);
        rightWallPress = (Physics2D.OverlapCircle(rightCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(rightCheck[1].transform.position, checkRadius, whatIsGround));
        leftWallPress = (Physics2D.OverlapCircle(leftCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(leftCheck[1].transform.position, checkRadius, whatIsGround));
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
        if (canShoot[currentWep])
        {
            if (shootInput)
            {
                GameObject bullet;
                BasicParticleScript script;
                //Debug.Log(aim);
                PoolerScript pS = PoolerScript.instance;
                Vector3 rotation = Vector3.zero;
                string tag = attackPrefabs[currentWep].tag;
                Vector3 position;
                switch (aim)
                {
                    case "right":
                        rotation.z = 0;
                        position = attackSpawns[0].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = script.velocity;
                        script.vertVel = 0;
                        break;
                    case "rightDown":
                        rotation.z = -45;
                        position = attackSpawns[1].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = script.velocity * Mathf.Cos(45);
                        script.vertVel = -script.velocity * Mathf.Cos(45);
                        break;
                    case "Down":
                        rotation.z = -90;
                        position = attackSpawns[2].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = 0;
                        script.vertVel = -script.velocity;
                        break;
                    case "leftDown":
                        rotation.z = -135;
                        position = attackSpawns[3].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = -script.velocity * Mathf.Cos(45);
                        script.vertVel = -script.velocity * Mathf.Cos(45);
                        break;
                    case "left":
                        rotation.z = -180;
                        position = attackSpawns[4].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = -script.velocity;
                        script.vertVel = 0;
                        break;
                    case "leftUp":
                        rotation.z = -225;
                        position = attackSpawns[5].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = -script.velocity * Mathf.Cos(45);
                        script.vertVel = script.velocity * Mathf.Cos(45);
                        break;
                    case "Up":
                        rotation.z = -270;
                        position = attackSpawns[6].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = 0;
                        script.vertVel = script.velocity;
                        break;
                    case "rightUp":
                        rotation.z = -315;
                        position = attackSpawns[7].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = script.velocity * Mathf.Cos(45);
                        script.vertVel = script.velocity * Mathf.Cos(45);
                        break;
                    default:
                        rotation.z = 0;
                        position = attackSpawns[0].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        bullet = pS.objectToSpawn;
                        script = bullet.GetComponent<BasicParticleScript>();
                        script.horiVel = script.velocity;
                        script.vertVel = 0;
                        break;
                }
                //Debug.Log(rotation.z);
                canShoot[currentWep] = false;
                StartCoroutine(ReadyToShoot(currentWep));
            }
        }
    }

    IEnumerator ReadyToShoot(int whichOne)
    {
        yield return new WaitForSeconds(attackCooldowns[whichOne]);

        //Debug.Log(whichOne);

        canShoot[whichOne] = true;
    }

    //Used to set up arrays on start that are not initialized before play
    //Currently only used for private canShoot array
    void SetUpArrays()
    {
        canShoot = new bool[attackCooldowns.Length];
    }

    void ResetCooldowns()
    {
        for(int x = 0; x < canShoot.Length; x++)
        {
            canShoot[x] = true;
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
            rb.gravityScale = reducedGravity;
        }
        else
        {
            rb.gravityScale = gravity;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpStrength);
    }

    private void WallJump(int direction)
    {
        rb.velocity = new Vector2((moveSpeed * direction), jumpStrength);
        control = false;
        if(horiAim == "right")
        {
            horiAim = "left";
        }
        else if (horiAim == "left")
        {
            horiAim = "right";
        }
        Invoke("ReturnControl", 0.5f);
    }

    private void ReturnControl()
    {
        control = true;
    }

}