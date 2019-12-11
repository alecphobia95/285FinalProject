using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechScript : MonoBehaviour
{
    public Animator animController;
    public GameObject mech;

    public static MechScript instance;

    //Remember to set this up on player script later
    public CameraMoveScript cameraScript;
    public GameObject player;

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
    public Transform playerCheck;
    public float checkRadius;
    public float checkMookRadius;
    public float checkPlayerRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsMook;
    public LayerMask whatIsPlayer;

    public Transform[] attackSpawns;
    public GameObject[] attackPrefabs;
    public float[] attackCooldowns;
    private bool[] canShoot;
    private int currentWep;

    public bool slippery;
    private bool control, dashing, canDash, piloting;
    private bool onGround, onMook, rightWallPress, leftWallPress, canSwitch;
    private bool leftInput, rightInput, upInput, downInput, leftDashInput, rightDashInput,
        jumpInput, jumpHold, shootInput, switchWepInput, pilotInput;
    private int airJumps, direction;
    private string horiAim, vertAim, aim;

    public float drainRate, refuelRate;
    private float fuelSupply;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        onGround = false;
        control = true;
        canDash = true;
        piloting = false;
        direction = 1;
        fuelSupply = 100;
        horiAim = "right";
        vertAim = "";
        currentWep = 0;
        rb.gravityScale = gravity;
        SetUpArrays();
        ResetCooldowns();
        SetCamMove();
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
            Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, whatIsPlayer));
        rightWallPress = (Physics2D.OverlapCircle(rightCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(rightCheck[1].transform.position, checkRadius, whatIsGround));
        leftWallPress = (Physics2D.OverlapCircle(leftCheck[0].transform.position, checkRadius, whatIsGround) ||
            Physics2D.OverlapCircle(leftCheck[1].transform.position, checkRadius, whatIsGround));
        canSwitch = Physics2D.OverlapCircle(playerCheck.transform.position, checkPlayerRadius, whatIsPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        GrabInputs();
        if (piloting)
        {
            AnimationUpdates();
            if (onGround)
            {
                airJumps = maxJumps;
            }
            SetPlayerAim();
            CurrentWeaponSelect();
            HandleShooting();
            RegularMovment();
            SetSpriteDirection();
        }
        PilotSwitchCheck();
        ClearInputs();
    }

    void AnimationUpdates()
    {
        ///To the Right
        if ((rightInput || leftInput) && onGround)
        {
            animController.SetBool("isGoingRight", true);
            //animController.SetBool("isIdling", false);
            //Debug.Log("To The Right");
        }

        else
        {
            animController.SetBool("isGoingRight", false);
            //animController.SetBool("isIdling", false);
            //Debug.Log("Right Stop");
        }

        //Jump On It
        if (jumpHold && !onGround)
        {
            animController.SetBool("isJumping", true);
        }

        else
        {
            animController.SetBool("isJumping", false);

        }

        if ((rightInput && rightWallPress && !onGround) || (leftInput && leftWallPress && !onGround))
        {
            //put wall cling on here
            animController.SetBool("isClinging", true);
            //Debug.Log("Hang in There!");
            if (jumpHold)
            {
                animController.SetBool("isJumping", false);
            }
        }

        else
        {
            //put wall cling off here
            animController.SetBool("isClinging", false);
            //Debug.Log("Or Die. Who cares.");
        }

        /////Put 'Em Up
        //if (shootInput)
        //{
        //    animController.SetBool("isShooting", true);
        //    Debug.Log("Enter has been pressed");
        //}

        //if (!shootInput)
        //{
        //    animController.SetBool("isShooting", false);
        //    Debug.Log("Enter has been released");
        //}

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

        if (Input.GetKey(KeyCode.Return))
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
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pilotInput = true;
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
        pilotInput = false;
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

        if (vertAim != "" && (!leftInput && !rightInput))
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
        if (shootInput)
        {
            fuelSupply -= drainRate * Time.deltaTime;
            if(fuelSupply < (-drainRate * Time.deltaTime) * 4)
            {
                fuelSupply = refuelRate * Time.deltaTime;
            }
            if (canShoot[currentWep] && fuelSupply > 0)
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
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
                            script.CancelInvoke("HideMe");
                            script.HideMe();
                        }
                        break;
                }
                //Debug.Log(rotation.z);
                canShoot[currentWep] = false;
                StartCoroutine(ReadyToShoot(currentWep));
            }
        }
        else {
            fuelSupply += refuelRate * Time.deltaTime;
            if(fuelSupply > 100)
            {
                fuelSupply = 100;
            }
        }
        FuelMeterScript.instance.fuelStatus = fuelSupply;
        //Debug.Log(fuelSupply);
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
        for (int x = 0; x < canShoot.Length; x++)
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
            }
            if (!dashing && ((!leftInput && !rightInput) || (!leftInput && !rightInput)) && control && !slippery)
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
            if (leftWallPress || rightWallPress)
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

    void SetSpriteDirection()
    {
        Vector3 whichFlip = player.transform.localScale;
        if (horiAim == "right")
        {
            if (whichFlip.x < 0)
            {
                whichFlip.x = -whichFlip.x;
                player.transform.localScale = whichFlip;
            }
            Debug.Log("Flip it real good");
        }
        if (horiAim == "left")
        {
            if (whichFlip.x > 0)
            {
                whichFlip.x = -whichFlip.x;
                player.transform.localScale = whichFlip;
            }
            Debug.Log("Flip it real good");
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
        if (horiAim == "right")
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

    private void SetCamMove()
    {
        if (piloting)
        {
            cameraScript.enabled = true;
        }
        else
        {
            cameraScript.enabled = false;
        }
    }

    private void PilotSwitchCheck()
    {
        if (pilotInput)
        {
            if (piloting)
            {
                Vector3 currentPosAdjust = this.transform.position;
                currentPosAdjust.y += 1;
                player.transform.position = currentPosAdjust;
                player.SetActive(true);
                piloting = false;
                PlayerScript.instance.piloting = false;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (!piloting)
            {
                if (canSwitch)
                {
                    piloting = true;
                    PlayerScript.instance.piloting = true;
                    player.SetActive(false);
                }
            }
            SetCamMove();
            PlayerScript.instance.SetCamMove();
        }
    }

    public void Death()
    {

    }
}
