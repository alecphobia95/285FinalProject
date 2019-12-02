﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //ANIMATION
    Animator animController;
    public GameObject player;
    [HideInInspector] public bool facingRight = true;

    public static PlayerScript instance;

    public CameraMoveScript cameraScript;

    public float moveSpeed;
    public float jumpStrength;
    public float dashDuration;
    public int maxJumps;
    public float gravity, reducedGravity;
    public Rigidbody2D rb;

    public Transform groundCheck;
    public Transform[] rightCheck;
    public Transform[] leftCheck;
    public Transform enemyCheck;
    public float checkRadius;
    public float checkMookRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsMook;
    public LayerMask whatIsMech;

    public Transform[] attackSpawns;
    public GameObject[] attackPrefabs;
    public float[] attackCooldowns;
    private bool[] canShoot;
    private int currentWep;

    public bool slippery;
    private bool control, dashing, canDash;
    public bool piloting;
    private bool onGround, onMook, rightWallPress, leftWallPress;
    private bool leftInput, rightInput, upInput, downInput, leftDashInput, rightDashInput,
        jumpInput, jumpHold, shootInput, switchWepInput;
    private int airJumps, direction;
    private string horiAim, vertAim, aim;

    // Use this for initialization
    void Start()
    {
        onGround = false;
        control = true;
        canDash = true;
        piloting = false;
        direction = 1;
        horiAim = "right";
        vertAim = "";
        currentWep = 0;
        rb.gravityScale = gravity;
        SetUpArrays();
        ResetCooldowns();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        onGround = (Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, whatIsMech));
        onMook = Physics2D.OverlapCircle(enemyCheck.transform.position, checkMookRadius, whatIsMook);
        rightWallPress = (Physics2D.OverlapCircle(rightCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(rightCheck[1].transform.position, checkRadius, whatIsGround)) ||
            (Physics2D.OverlapCircle(rightCheck[0].transform.position, checkRadius, whatIsMech) ||
            Physics2D.OverlapCircle(rightCheck[1].transform.position, checkRadius, whatIsMech));
        leftWallPress = (Physics2D.OverlapCircle(leftCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(leftCheck[1].transform.position, checkRadius, whatIsGround)) ||
            (Physics2D.OverlapCircle(leftCheck[0].transform.position, checkRadius, whatIsMech) ||
            Physics2D.OverlapCircle(leftCheck[1].transform.position, checkRadius, whatIsMech));
    }

    // Update is called once per frame
    void Update()
    {
        if (!piloting)
        {
            GrabInputs();
            if (onGround)
            {
                airJumps = maxJumps;
            }
            SetPlayerAim();
            CurrentWeaponSelect();
            HandleShooting();
            RegularMovment();
            ClearInputs();
        }

        AnimationUpdates();
    }
    /// <summary>
    void AnimationUpdates()
    {
        animController = GetComponent<Animator>();
        ///To the Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            animController.SetBool("isGoingRight", true);
            //animController.SetBool("isIdling", false);
            Debug.Log("To The Right");
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animController.SetBool("isGoingRight", false);
            //animController.SetBool("isIdling", false);
            Debug.Log("Right Stop");
        }

        ///To the Left
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animController.SetBool("isGoingRight", true);
            //animController.SetBool("isIdling", false);
            Debug.Log("To The Left");
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animController.SetBool("isGoingRight", false);
            //animController.SetBool("isIdling", false);
            Debug.Log("Right Stop");
        }


        //Jump On It
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animController.SetBool("isJumping", false);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animController.SetBool("isJumping", true);
        }

        ///Put 'Em Up
        if (Input.GetKeyDown(KeyCode.Return))
        {
            animController.SetBool("isShooting", true);
            Debug.Log("Enter has been pressed");
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            animController.SetBool("isShooting", false);
            Debug.Log("Enter has been released");
        }

    }
    /// </summary>
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchWepInput = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftDashInput = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rightDashInput = true;
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
        shootInput = false;
        switchWepInput = false;
        leftDashInput = false;
        rightDashInput = false;
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

    void CurrentWeaponSelect()
    {
        if (switchWepInput)
        {
            currentWep++;
            if (currentWep >= attackPrefabs.Length)
            {
                currentWep = 0;
            }
            TempUIScript.instance.CurrentWep(currentWep);
        }
    }

    void HandleShooting()
    {
        if (canShoot[currentWep])
        {
            if (shootInput)
            {
                GameObject attack;
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
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = script.velocity;
                            script.vertVel = 0;
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "rightDown":
                        rotation.z = -45;
                        position = attackSpawns[1].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = script.velocity * Mathf.Cos(45);
                            script.vertVel = -script.velocity * Mathf.Cos(45);
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "Down":
                        rotation.z = -90;
                        position = attackSpawns[2].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = 0;
                            script.vertVel = -script.velocity;
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "leftDown":
                        rotation.z = -135;
                        position = attackSpawns[3].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = -script.velocity * Mathf.Cos(45);
                            script.vertVel = -script.velocity * Mathf.Cos(45);
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "left":
                        rotation.z = -180;
                        position = attackSpawns[4].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = -script.velocity;
                            script.vertVel = 0;
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "leftUp":
                        rotation.z = -225;
                        position = attackSpawns[5].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = -script.velocity * Mathf.Cos(45);
                            script.vertVel = script.velocity * Mathf.Cos(45);
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "Up":
                        rotation.z = -270;
                        position = attackSpawns[6].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = 0;
                            script.vertVel = script.velocity;
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    case "rightUp":
                        rotation.z = -315;
                        position = attackSpawns[7].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = script.velocity * Mathf.Cos(45);
                            script.vertVel = script.velocity * Mathf.Cos(45);
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
                        break;
                    default:
                        rotation.z = 0;
                        position = attackSpawns[0].position;
                        pS.SpawnFromPool(tag, position, rotation);
                        attack = pS.objectToSpawn;
                        if (attack.name.Contains("Ranged"))
                        {
                            BasicParticleScript script = attack.GetComponent<BasicParticleScript>();
                            script.horiVel = script.velocity;
                            script.vertVel = 0;
                        }
                        if (attack.name.Contains("Melee"))
                        {
                            MeleeAttackScript script = attack.GetComponent<MeleeAttackScript>();
                            script.HideMe();
                        }
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
            dashing = false;
            canDash = true;
            CancelInvoke("EnableDash");
        }
        if (jumpInput && !onGround && airJumps > 0 && !rightWallPress && !leftWallPress)
        {
            Jump();
            airJumps--;
            control = true;
            dashing = false;
            canDash = true;
            CancelInvoke("EnableDash");
        }
        //if you wish to require input toward wall to walljump then add && rightInput
        if (jumpInput && !onGround && rightWallPress && !leftWallPress)
        {
            direction = -1;
            WallJump();
            dashing = false;
            canDash = true;
            CancelInvoke("EnableDash");
        }
        //if you wish to require input toward wall to walljump then add && leftInput
        if (jumpInput && !onGround && !rightWallPress && leftWallPress)
        {
            direction = 1;
            WallJump();
            dashing = false;
            canDash = true;
            CancelInvoke("EnableDash");
        }
        if (control == true || onGround)
        {
            //Debug.Log("Good on walk");
            if (leftInput && !leftWallPress)
            {
                //Debug.Log("Initiating lateral movement");
                horiAim = "left";
                if (!dashing)
                {
                    StopDash();
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                    direction = 1;
                }

                ///
                if (facingRight)
                {
                    facingRight = false;
                    player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
                    Debug.Log("Flip it real good");
                }
                ///
            }
            if (rightInput && !rightWallPress)
            {
                //Debug.Log("Initiating lateral movement");
                horiAim = "right";
                if (!dashing)
                {
                    StopDash();
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                    direction = -1;
                }

                ///
                if (!facingRight)
                {
                    facingRight = true;
                    player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
                    Debug.Log("Flip it real good");
                }
                ///
            }
            if(!dashing && ((!leftInput && !rightInput) || (!leftInput && !rightInput)) && control && !slippery)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (leftInput && leftWallPress)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                horiAim = "right";
            }
            if (rightInput && rightWallPress)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                horiAim = "left";
            }

            if (canDash)
            {
                if ((leftDashInput && !rightDashInput) || (!leftDashInput && rightDashInput))
                {
                    dashing = true;
                    control = false;
                    canDash = false;

                    if (rightDashInput)
                    {
                        direction = 1;
                        horiAim = "right";
                    }
                    if (leftDashInput)
                    {
                        direction = -1;
                        horiAim = "left";
                    }

                    rb.gravityScale = 0f;
                    
                    Invoke("ReturnControl", dashDuration);
                    Invoke("StopDash", dashDuration);
                    Invoke("EnableDash", dashDuration * 1.5f);
                }
            }
        }

        if (dashing)
        {
            if(leftWallPress || rightWallPress)
            {
                StopDash();
            }
            rb.velocity = new Vector2(moveSpeed * 1.5f * direction, 0f);
        }

        else if (!dashing)
        {
            if (rb.velocity.y > 0f && jumpHold)
            {
                rb.gravityScale = reducedGravity;
            }
            else
            {
                rb.gravityScale = gravity;
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(0, jumpStrength);
    }

    private void WallJump()
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
        CancelInvoke("ReturnControl");
        Invoke("ReturnControl", 0.5f);
    }

    private void ReturnControl()
    {
        control = true;
    }

    private void EnableDash()
    {
        canDash = true;
    }

    private void StopDash()
    {
        control = true;
        dashing = false;
        rb.gravityScale = gravity;
    }

    public void SetCamMove()
    {
        if (!piloting)
        {
            cameraScript.enabled = true;
        }
        else
        {
            cameraScript.enabled = false;
        }
    }

}