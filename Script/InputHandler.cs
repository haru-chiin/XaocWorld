using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool a_Input;
    public bool b_Input;
    public bool x_input;
    public bool y_input;
    public bool critial_Attack_Input;
    public bool rollFlag;
    public float rollInputTimer;
    public bool jump_input;
    public bool inventory_input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;
    public bool parry_inputl;
    public bool blocking_input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    public bool lAInput;
    public bool HAInput;
    
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    public bool twoHandedFlag;

    public Transform criticalAttackRayCastStartPoint;

    PlayerControls inputActions;
    cameraHandler cameraHandler;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uI;
    WeaponSlotManager WeaponSlotManager;
    PlayerAnimatorHandler animatorHandler;
    PlayerStats playerStats;
    PlayerEffectManager playerEffectManager;
    PlayerAnimatorHandler playerAnimatorHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        cameraHandler = FindAnyObjectByType<cameraHandler>();
        uI = FindAnyObjectByType<UIManager>();
        WeaponSlotManager = GetComponent<WeaponSlotManager>();
        animatorHandler = GetComponent<PlayerAnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
        playerEffectManager = GetComponent<PlayerEffectManager>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
    }

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
            inputActions.PlayerActions.LAttack.performed += i => lAInput = true;
            inputActions.PlayerActions.HAttack.performed += i => HAInput = true;
            inputActions.PlayerQuickSlot.DpadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlot.DpadLeft.performed += i => d_Pad_Left = true;
            inputActions.PlayerActions.A.performed += inputActions => a_Input = true;
            inputActions.PlayerActions.Jump.performed += inputActions => jump_input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;
            inputActions.PlayerActions.Y.performed += i => y_input = true;
            inputActions.PlayerActions.CriticalAttack.performed += i => critial_Attack_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            inputActions.PlayerActions.Parry.performed += i => parry_inputl = true;
            inputActions.PlayerActions.Shield.performed += i => blocking_input = true;
            inputActions.PlayerActions.X.performed += i => x_input = true;

        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        HandlemoveInput(delta);
        HandleRollingInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotInput();
/*        HandleInteractablenButtonInput();
        HandleJumpInput();*/
        HandleLockOnInput();
        HandleInventoryInput();
        HandleTwoHandedInput();
        HandleCriticalAttackInput();
        HandleUseConsumeableInput();
    }

    private void HandlemoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollingInput(float delta)
    {
        /*b_Input = inputActions.PlayerActions.Roll.IsPressed();*/


        if (b_Input)
        {
            rollInputTimer += delta;
            if(playerStats.CurrentStamina <= 0)
            {
                b_Input = false;
                sprintFlag = false;
            }
            if(moveAmount > 0.5f && playerStats.CurrentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;

            if(rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                /*sprintFlag = false;*/
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleCombatInput(float delta)
    {
        /*        inputActions.PlayerActions.LAttack.performed += i => lAInput = true;
                inputActions.PlayerActions.HAttack.performed += i => HAInput = true;*/

        if (inventoryFlag)
        {
            Debug.Log("Input Dihentikan: Inventory aktif"); // Tambahkan log ini untuk debugging
            return; // Abaikan input serangan jika inventory aktif
        }

        if (lAInput)
        {
            playerAttacker.HandleLAAction();
   
        }

        if (blocking_input)
        {

        }

        if (HAInput)
        {

            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        if (parry_inputl)
        {
            if (twoHandedFlag)
            {

            }
            else
            {
                playerAttacker.HandleParryAction();
            }
        }
    }

    private void HandleQuickSlotInput()
    {
/*        inputActions.PlayerQuickSlot.DpadRight.performed += i => d_Pad_Right = true;
        inputActions.PlayerQuickSlot.DpadLeft.performed += i => d_Pad_Left = true;*/
        if (d_Pad_Right)
        {
            playerInventory.ChangeRightHandWeapon();
        }
        else if (d_Pad_Left)
        {
            playerInventory.ChangeLeftHandWeapon();
        }
    }

/*    private void HandleInteractablenButtonInput()
    {
        inputActions.PlayerActions.A.performed += inputActions => a_Input = true;
    }*/

/*    private void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += inputActions => jump_input = true;
    }*/

    private void HandleInventoryInput()
    {
        /*inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;*/

        if (inventory_input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uI.OpenSelectedWindow();
                uI.UpdateUI();
                uI.hudWindow.SetActive(false);
            }
            else
            {
                uI.ClosedSelectedWindow();
                uI.closeAllInventoryWindows();
                uI.hudWindow.SetActive(true);
            }
            Debug.Log("Inventory Flag: " + inventoryFlag);
        }
    }

    private void HandleLockOnInput()
    {
        if(lockOnInput && lockOnFlag == false)
        {
            cameraHandler.ClearLockOnTarget();
            lockOnInput = false;
            cameraHandler.HandleLock();
            if(cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if( lockOnInput && lockOnFlag)
        {
            lockOnFlag = false;
            lockOnInput = false;
            cameraHandler.ClearLockOnTarget();
        }

        if(lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            cameraHandler.HandleLock();
            if(cameraHandler.leftLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }

        if(lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            cameraHandler.HandleLock();
            if(cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }

        cameraHandler.setCameraHeight();
    }

    private void HandleTwoHandedInput()
    {
        if (y_input)
        {
            y_input = false;
            twoHandedFlag = !twoHandedFlag;

            if (twoHandedFlag)
            {
                WeaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                playerManager.isTwoHandingWeapon = true;
            }
            else
            {
                WeaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                WeaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                playerManager.isTwoHandingWeapon = false;
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (critial_Attack_Input)
        {
            critial_Attack_Input = false;
            playerAttacker.AttemptBackStabOrRiposte();
        }
    }

    private void HandleUseConsumeableInput()
    {
        if (x_input)
        {
            x_input = false;
            playerInventory.currentConsumable.AttemptToConsumeItem(playerAnimatorHandler, WeaponSlotManager, playerEffectManager);
        }
    }
}
