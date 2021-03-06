using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public GameObject smokeTrailer;

    [Header("References")]
    public Transform colliderTransform;
    public Transform playerCentre;
    public Rigidbody rb;
    public Animator animator;

    [Header("Movement")]
    public float runSpeed=6f;
    public float movementMultiplier = 10f;
    public float blackholemovementMultiplier = 10f;
    public float airMultiplier = 0.4f;
    public float rampJumpMultiplier = 0.4f;
    public float superJumpAirMultiplier = 0.8f;
    public float currentAirMultiplier;
    public float currentMovementMultiplier;
    bool isPlummitingForLaneSwitch;
    public bool isEnabled;

    [Header("EnableAbility")]
    public bool canFireLaser;
    public bool canEnableShield;

    [Header("Special Mechanic")]
    [Header("SuperJump")]
    public float superJumpTime;
    bool isRunningSuperJumpTimer;
    float superJumpTimeTemp;

    [Header("Skate board-laser")]
    public int currentProjectiles;
    public int projectileLimit=3;
    public float replenishTimeForOneProjectile = 4f;
    public float replenishTimeForOneProjectileTemp;
    public Transform projectileSpawnPoint;

    [Header("Shield")]
    public Shield shield;
    public float shieldTime;
    public float shieldTimeTemp;
    public bool isShieldOn;

    [Header("Special Obstacle")]
    [Header("WaterPuddle")]
    public float slipTime=3f;
    public float slipTimeTemp;
    public bool isSlipping;

    [Header("Ramp")]
    public float launchAngle=45;
    public float launchForce=100;
    public float rampSnapMoveSpeed;
    public bool isSnappingToRampApex;

    [Header("Blackhole")]
    public float blackHoleTime=3;
    public float blackHoleTimeTemp;
    public bool isRunningBlackHoleTimer;

    [Header("Jumping")]
    public float jumpForce=15f;
    public float superJumpForce = 30f;

    [Header("Crouch")]
    public float plumitForce=5f;
    public float falsePlumitOnLaneSwitchForce=5f;
    public float crouchTime;
    float crouchTemp;
    bool isCrouched;
    bool isPlummitingForCrouch;

    [Header("GroundDetection")]
    bool isGrounded;
    public float playerHeight=2f;

    [Header("Drag")]
    public float groundDrag=6f;
    public float airDrag=2f;

    [Header("LaneSwitch")]
    public float laneDistance=2.5f;
    public float switchLaneSpeed=10f;
    public int currentLane = 1;
    float transitionX;
    float targetX;
    bool switchLane;

    [Header("InVulnerable")]
    public float invulnerableTime;
    public float invulnerableTimeTemp;
    public bool isInvulnerable;

    private void Awake()
    {
        instance = this;
    }

    public void HidePlayer()
    {
        animator.gameObject.SetActive(false);
    }

    public void ShowPlayer()
    {
        animator.gameObject.SetActive(true);
    }

    private void Start()
    {
        currentProjectiles = projectileLimit;
        GameManager.instance.fireText.text = currentProjectiles.ToString();
        currentMovementMultiplier = movementMultiplier;
        currentAirMultiplier = airMultiplier;
    }

    private void Update()
    {
        PerformActions();
        PerformAnimations();
        RunInvulnerablilityTimer();
       // RunCrouchTimer();
        RunSuperJumpTimer();
        RunSlipTimer();
        RunProjectileReplenshmentTimer();
        RunShieldTimer();
        RunBlackHoleTimer();

        if (isGrounded)
        {
            smokeTrailer.SetActive(true);
        }
        else
        {
            smokeTrailer.SetActive(false);
        }

        if (tempCooldownShield>0)
        {
            tempCooldownShield -= Time.deltaTime;
            if (tempCooldownShield < 0)
            {
                tempCooldownShield = 0;
            }
            GameManager.instance.shieldTimer.fillAmount = tempCooldownShield / maxCooldownShield;
        }
        else
        {
            GameManager.instance.shieldTimer.fillAmount = 0 / maxCooldownShield;
        }
    }

    void PerformAnimations()
    {
        animator.SetBool("air",!isGrounded);
        animator.SetBool("slide",isCrouched);
        animator.speed = isRunningBlackHoleTimer == true ? 2f : 1f;
    }

    void RunBlackHoleTimer()
    {
        if (isRunningBlackHoleTimer)
        {
            speedFX.SetActive(true);
            if (blackHoleTimeTemp >= blackHoleTime)
            {
                isRunningBlackHoleTimer = false;
                blackHoleTimeTemp = 0;
                currentMovementMultiplier = movementMultiplier;
            }
            else
            {
                blackHoleTimeTemp += Time.deltaTime;
            }
        }
        else
        {
            speedFX.SetActive(false);
        }
    }

    void RunShieldTimer()
    {
        if (isShieldOn)
        {
            if (shieldTimeTemp >= shieldTime)
            {
                isShieldOn = false;
                shieldTimeTemp = 0;

                shield.Close();
            }
            else
            {
                shieldTimeTemp += Time.deltaTime;
            }
        }
    }
    float maxCooldownShield=10;
    float tempCooldownShield=0;

    IEnumerator cor;

    public void TravelToRampLaunchPoint(Transform startPoint,Transform launchPoint)
    {
        if(cor==null)
        {
            if(!isSnappingToRampApex)
            {
                rb.isKinematic = true;
                cor = FixedTravelCor(startPoint.position,launchPoint.position);
                StartCoroutine(cor);
                isSnappingToRampApex = true;
            }
        }
    }

    IEnumerator FixedTravelCor(Vector3 startPoint,Vector3 launchPointVector3)
    {
        while (Vector3.Distance(transform.position, launchPointVector3)>=0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, launchPointVector3,Time.deltaTime*rampSnapMoveSpeed);
            yield return null;
        }
        transform.position = launchPointVector3;
        cor = null;
        isSnappingToRampApex = false;

        rb.isKinematic = false;
        Quaternion rot = Quaternion.AngleAxis(launchAngle,Vector3.right);
        Vector3 direction = Vector3.up;

        Debug.DrawRay(transform.position,rot*direction,Color.blue);

        RampJump(rot * direction);
        yield break;
    }

    public void IncreasePlayerMoveSpeed()
    {
        currentMovementMultiplier = blackholemovementMultiplier;
        isRunningBlackHoleTimer = true;
    }

    public void SlipPlayer()
    {
        isSlipping = true;
        animator.SetBool("air", false);
        animator.SetBool("slide", false);
        animator.SetBool("slip", true);
    }

    public void FireLaserProjectile()
    {
        if (currentProjectiles > 0)
        {
            ProjectileSpawnner.instance.SpawnProjectile(projectileSpawnPoint.position);
            currentProjectiles--;
            GameManager.instance.fireText.text = currentProjectiles.ToString();
        }
    }

    void RunProjectileReplenshmentTimer()
    {
        if(currentProjectiles<projectileLimit)
        {
            if (replenishTimeForOneProjectileTemp >= replenishTimeForOneProjectile)
            {
                currentProjectiles++;
                replenishTimeForOneProjectileTemp = 0;
                GameManager.instance.fireText.text = currentProjectiles.ToString();
            }
            else
            {
                replenishTimeForOneProjectileTemp += Time.deltaTime;
            }
        }
    }

    void RunSlipTimer()
    {
        if (isSlipping)
        {
            if (slipTimeTemp >= slipTime)
            {
                isSlipping = false;
                animator.SetBool("slip", false);
                animator.SetBool("air", !isGrounded);
                animator.SetBool("slide", isCrouched);
                slipTimeTemp = 0;
            }
            else
            {
                slipTimeTemp += Time.deltaTime;
            }
        }
    }
    public GameObject speedFX;

    void RunInvulnerablilityTimer()
    {
        if (isInvulnerable)
        {
            if (invulnerableTimeTemp >= invulnerableTime)
            {
                isInvulnerable = false;
                invulfx.SetActive(false);
                invulnerableTimeTemp = 0;
            }
            else
            {
                invulnerableTimeTemp += Time.deltaTime;
            }
        }
    }

    void RunSuperJumpTimer()
    {
        if (isRunningSuperJumpTimer)
        {
            if (superJumpTimeTemp >= superJumpTime)
            {
                isRunningSuperJumpTimer = false;
                superJumpTimeTemp = 0;
            }
            else
            {
                superJumpTimeTemp += Time.deltaTime;
            }
        }
    }

    void RampJump(Vector3 direction)
    {
        if (isCrouched)
        {
            UnCrouch();
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(direction * launchForce, ForceMode.Impulse);
        currentAirMultiplier = rampJumpMultiplier;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentAirMultiplier = airMultiplier;
    }

    void Superjump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);

        isRunningSuperJumpTimer = true;

        currentAirMultiplier = superJumpAirMultiplier;
    }


    void RunLaneSwitch()
    {
        if(switchLane)
        {
            if (Mathf.Abs(rb.transform.position.x- targetX) >=0.1f)
            {
                transitionX = Mathf.MoveTowards(rb.transform.position.x, targetX, Time.fixedDeltaTime * switchLaneSpeed);
                rb.MovePosition(new Vector3(transitionX, rb.transform.position.y, rb.transform.position.z));
            }
            else
            {
                rb.MovePosition(new Vector3(targetX, rb.transform.position.y, rb.transform.position.z));
                switchLane = false;
            }
        }
    }

    void RunCrouchTimer()
    {
        if (isCrouched)
        {
            crouchTemp += Time.deltaTime;
            if (crouchTemp >= crouchTime)
            {
                UnCrouch();
            }
        }
    }

    void Move()
    {
        if(isGrounded)
        {
            rb.AddForce(rb.transform.forward * runSpeed * currentMovementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(rb.transform.forward * runSpeed * currentAirMultiplier, ForceMode.Acceleration);
        }
    }
    public GameObject invulfx;

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(playerCentre.position, Vector3.down,playerHeight/2+0.1f);
        Debug.DrawLine(playerCentre.position, playerCentre.position+Vector3.down*(playerHeight / 2 + 0.1f),Color.red);

        //if(isGrounded&&!rb.isKinematic)
        //{
        //    rb.transform.position = new Vector3(rb.transform.position.x, 0, rb.transform.position.z);
        //}

        if(isPlummitingForCrouch&&isGrounded)
        {
            isPlummitingForCrouch = false;
            Crouch();
        }

        if (isPlummitingForLaneSwitch && isGrounded)
        {
            isPlummitingForLaneSwitch = false;
        }
    }

    void PerformActions()
    {
        if(isSlipping||isSnappingToRampApex)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            WPressed();
        }
        else if(Input.GetKeyDown(KeyCode.Space) )
        {
            SpacePressed();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SPressed();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ShieldPressed();
        }
        else if(canFireLaser&&Input.GetKeyDown(KeyCode.X))
        {
                FireLaserProjectile();
        }
        if(!switchLane)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                APressed();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                DPressed();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            SUp();
        }
    }

    public void ShieldPressed()
    {
        if (!isShieldOn && canEnableShield)
        {
            if (tempCooldownShield > 0f)
            {
                return;
            }

            isShieldOn = true;
            shield.Initialise();
            tempCooldownShield = maxCooldownShield;
        }
    }

    public void APressed()
    {
        //move left
        MoveLane(false);
        CalculateTargetXPosition();
        switchLane = true;
    }

    public void SpacePressed()
    {
        if (isGrounded && !isRunningSuperJumpTimer)
        {
            if (isCrouched)
            {
                UnCrouch();
            }
            Superjump();
        }
    }

    public void WPressed()
    {
        if (isGrounded)
        {
            if (isCrouched)
            {
                UnCrouch();
            }
            Jump();
        }
    }

    public void DPressed()
    {
        //move right
        MoveLane(true);
        CalculateTargetXPosition();
        switchLane = true;
    }

    public void SPressed()
    {
        if (isGrounded)
        {
            Crouch();
        }
        else
        {
            //drop down fast & trigger crouch on land
            //on grounded trigger crouch
            Plumit();
        }
    }

    public void SUp()
    {
        UnCrouch();
    }

    void CalculateTargetXPosition()
    {
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(currentLane==0)
        {
            targetPosition += Vector3.left * laneDistance;
            //false plumit here
            FalsePlumitOnLaneSwitch();
        }
        else if(currentLane==2)
        {
            targetPosition += Vector3.right * laneDistance;
            //false plumit here
            FalsePlumitOnLaneSwitch();
        }
        else if(currentLane ==1)
        {
            FalsePlumitOnLaneSwitch();
        }
        targetX = targetPosition.x;
    }

    void MoveLane(bool goingRight)
    {
        currentLane += (goingRight) ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane,0,2);
    }

    void Plumit()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(-Vector3.up * plumitForce, ForceMode.Impulse);
        isPlummitingForCrouch = true;
    }

    void FalsePlumitOnLaneSwitch()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(-Vector3.up * falsePlumitOnLaneSwitchForce, ForceMode.Impulse);
        isPlummitingForLaneSwitch = true;
    }


    void Crouch()
    {
        if(isCrouched)
        {
            return;
        }
        rb.transform.position = new Vector3(rb.transform.position.x, 0, rb.transform.position.z);
        colliderTransform.localScale = new Vector3(colliderTransform.localScale.x, playerHeight/ 2.8f, colliderTransform.localScale.z);
        isCrouched = true;
    }

    void UnCrouch()
    {
        colliderTransform.localScale = new Vector3(colliderTransform.localScale.x, playerHeight / 2f, colliderTransform.localScale.z);
        isCrouched = false;
        crouchTemp = 0;
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
        ControlDrag();
        RunLaneSwitch();
    }
}
