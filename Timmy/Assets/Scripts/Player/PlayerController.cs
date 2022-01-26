using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    [Header("Crouch")]
    public float plumitForce=5f;
    public float crouchTime;
    float crouchTemp;
    bool isCrouched;
    bool isPlummiting;

    [Header("GroundDetection")]
    bool isGrounded;
    public float playerHeight=2f;

    [Header("Drag")]
    public float groundDrag=6f;
    public float airDrag=2f;

    [Header("LaneSwitch")]
    public float laneDistance=2.5f;
    public float switchLaneSpeed=10f;
    int currentLane = 1;
    float transitionX;
    float targetX;
    bool switchLane;

    private void Start()
    {
        currentProjectiles = projectileLimit;
        currentMovementMultiplier = movementMultiplier;
        currentAirMultiplier = airMultiplier;
    }

    private void Update()
    {
        PerformActions();
        PerformAnimations();
        RunCrouchTimer();
        RunSuperJumpTimer();
        RunSlipTimer();
        RunProjectileReplenshmentTimer();
        RunShieldTimer();
        RunBlackHoleTimer();
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
    }

    void FireLaserProjectile()
    {
        ProjectileSpawnner.instance.SpawnProjectile(projectileSpawnPoint.position);
        currentProjectiles--;
    }

    void RunProjectileReplenshmentTimer()
    {
        if(currentProjectiles<projectileLimit)
        {
            if (replenishTimeForOneProjectileTemp >= replenishTimeForOneProjectile)
            {
                currentProjectiles++;
                replenishTimeForOneProjectileTemp = 0;
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
                slipTimeTemp = 0;
            }
            else
            {
                slipTimeTemp += Time.deltaTime;
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
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

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
        Debug.DrawLine(playerCentre.position, playerCentre.position+Vector3.down,Color.red);
        if(isPlummiting&&isGrounded)
        {
            isPlummiting = false;
            Crouch();
        }
    }

    void PerformActions()
    {
        if(isSlipping||isSnappingToRampApex)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            if (isCrouched)
            {
                UnCrouch();
            }
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isGrounded&&!isRunningSuperJumpTimer)
        {
            if (isCrouched)
            {
                UnCrouch();
            }
            Superjump();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(isGrounded)
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
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if(!isShieldOn)
            {
                isShieldOn = true;
                shield.Initialise();
            }
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            if (currentProjectiles > 0)
            {
                FireLaserProjectile();
            }
        }
        if(!switchLane)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //move left
                MoveLane(false);
                CalculateTargetXPosition();
                switchLane = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                //move right
                MoveLane(true);
                CalculateTargetXPosition();
                switchLane = true;
            }
        }
    }

    void CalculateTargetXPosition()
    {
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(currentLane==0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if(currentLane==2)
        {
            targetPosition += Vector3.right * laneDistance;
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
        isPlummiting = true;
    }


    void Crouch()
    {
        if(isCrouched)
        {
            return;
        }
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
