using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion2 : MonoBehaviour
{
    cameraHandler cameraHandler;
    Transform cameraObject;
    InputHandler inputHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public PlayerAnimatorHandler playerAnimatorHandler;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Ground and Air")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [Header("stats")]
    [SerializeField]
    float movementSpeed = 5;
/*    [SerializeField]
    float walkingSpeed = 1;*/
    [SerializeField]
    float rotationsSpeed = 10;
    [SerializeField]
    float sprintSpeed = 10;
    [SerializeField]
    float fallingSpeed = 45;

    [Header("Stamina cost")]
    [SerializeField]
    int rollStaminaCost = 15;
    int backStepStaminaCost = 10;
    int sprintStaminaCost = 1;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public float jumpForce = 250f;
    public bool jumpForceApplied;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<cameraHandler>();
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {

        cameraObject = Camera.main.transform;
        myTransform = transform;
        playerAnimatorHandler.Initialized();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleRotation(float delta)
    {
        if (playerAnimatorHandler.canRotate)
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationsSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.Normalize();
                    rotationDirection.y = 0;
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationsSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }

            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.moveAmount;

                targetDir = cameraObject.forward * inputHandler.vertical;
                targetDir += cameraObject.right * inputHandler.horizontal;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                {
                    targetDir = myTransform.forward;
                }

                float rs = rotationsSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRot = Quaternion.Slerp(myTransform.rotation, tr, rs * Time.deltaTime);

                myTransform.rotation = targetRot;
            }
        }
        

    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;

        if (playerManager.isInteracting)
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
            playerStats.TakeStamina(sprintStaminaCost);
        }
        else
        {
            moveDirection *= speed;
        }
        

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
        {
            playerAnimatorHandler.UpdateAnimatorValue(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
        }
        else
        {
            playerAnimatorHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }

        


    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimatorHandler.anim.GetBool("isInteracting"))
            return;

        if (playerStats.CurrentStamina <= 0)
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right* inputHandler.horizontal;

            if(inputHandler.moveAmount > 0)
            {
                playerAnimatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                playerStats.TakeStamina(rollStaminaCost);
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation("Backstep",true);
                playerStats.TakeStamina(backStepStaminaCost);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if(Physics.Raycast (origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 8f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if(inAirTimer > 0.5f)
                {
                    Debug.Log("aiirr");
                    playerAnimatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    playerAnimatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }
                playerManager.isInAir = false;
            }

        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if(playerManager.isInAir == false)
            {
                if(playerManager.isInteracting == false)
                {
                    playerAnimatorHandler.PlayTargetAnimation("Falling", true);
                }
                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isGrounded)
        {
            if(playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;
        if (playerStats.CurrentStamina <= 0)
            return;

        if (inputHandler.jump_input)
        {
            if(inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                playerAnimatorHandler.PlayTargetAnimation("Jump", true);
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;
                jumpForceApplied = true;
                Debug.Log("loncat");
            }
        }
    }
    private void FixedUpdate()
    {
        if (jumpForceApplied)
        {
            StartCoroutine(JumpCo());
            rigidbody.AddForce(transform.up * jumpForce);

        }
    }
    private IEnumerator JumpCo()
    {
        yield return new WaitForSeconds(0.35f);
        jumpForceApplied = false;
    }


    #endregion

}
