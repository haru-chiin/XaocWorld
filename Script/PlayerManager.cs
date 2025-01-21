using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public UIManager uIManager; 

    PlayerAnimatorHandler playerAnimatorHandler;
    Animator animator;
    InputHandler inputHandler;
    cameraHandler cameraHandler;
    PlayerLocomotion2 playerLocomotion;
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject ItemInteractableGameObject;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInteracting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingleftHand;
    public bool isInvulnerabel;

    private Coroutine hideItemCoroutine;


    private void Awake()
    {
        cameraHandler = cameraHandler.singleton;
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion2>();
        playerStats = GetComponent<PlayerStats>();
        interactableUI = FindObjectOfType<InteractableUI>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
        uIManager = FindObjectOfType<UIManager>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        

    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        animator.SetBool("isInAir", isInAir);
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingleftHand = animator.GetBool("isUsingLeftHand");
        isInvulnerabel = animator.GetBool("isInvulnerabel");
        animator.SetBool("isDead", playerStats.isDead);
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        playerAnimatorHandler.canRotate = animator.GetBool("canRotate");

        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        CheckForInteractableObject();

        playerStats.RegenerateStamina();
        playerLocomotion.HandleJumping();

    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleRotation(delta);
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        isSprinting = inputHandler.b_Input;
        inputHandler.lAInput = false;
        inputHandler.HAInput = false;
        inputHandler.d_Pad_Right = false;
        inputHandler.d_Pad_Left = false;
        inputHandler.d_Pad_Up = false;
        inputHandler.d_Pad_Down = false;
        inputHandler.a_Input = false;
        inputHandler.jump_input = false;
        inputHandler.inventory_input = false;
        inputHandler.parry_inputl = false; 

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.a_Input)
                    {
                        interactableObject.Interact(this);

                        if (ItemInteractableGameObject != null)
                        {
                            ItemInteractableGameObject.SetActive(true);

                            if (hideItemCoroutine != null)
                            {
                                StopCoroutine(hideItemCoroutine);
                            }
                            hideItemCoroutine = StartCoroutine(HideItemInteractableGameObjectAfterDelay(4f));
                        }
                    }
                }
            }
        }
        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }
        }
    }

    private IEnumerator HideItemInteractableGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ItemInteractableGameObject != null)
        {
            ItemInteractableGameObject.SetActive(false);
        }
    }

    public void OpenChestInteraction(Transform playerStandsHereWhenOpen)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero;
        transform.position = playerStandsHereWhenOpen.transform.position;
        playerAnimatorHandler.PlayTargetAnimation("Open Chest", true);
    }

    public void PassThroughFogWallInteraction(Transform fogWallEnterance)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEnterance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;

        playerAnimatorHandler.PlayTargetAnimation("Pass Through Fog", true);
    }

}
