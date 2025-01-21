using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot backSlot;
    PlayerInventory playerInventory;
    public DamageCollider leftHandDamage;
    public DamageCollider rightHandDamage;
    Animator animator;
    public WeaponItem unarmedWeapon;
    public WeaponItem attackingWeapon;
    PlayerEffectManager playerFXManager;
    PlayerAnimatorHandler playerAnimatorHandler;

    QuickSlotUI quick;
    PlayerStats playerStats;
    InputHandler inputHandler;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        quick = FindAnyObjectByType<QuickSlotUI>();
        inputHandler = GetComponent<InputHandler>();
        playerInventory = GetComponent<PlayerInventory>();
        playerFXManager = GetComponent<PlayerEffectManager>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
        LoadWeaponHolderSlots();
        
    }

    private void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot.isLeftHandSlot)
            {
                leftHandSlot = weaponslot;
            }
            else if (weaponslot.isRightHandSlot)
            {
                rightHandSlot = weaponslot;
            }
            else if (weaponslot.isBackslot)
            {
                backSlot = weaponslot;
            }
        }

    }

    public void LoadBothWeaponOnSlot()
    {
        LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        LoadWeaponOnSlot(playerInventory.leftWeapon, true);
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quick.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

            }
            else
            {
                if (inputHandler.twoHandedFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    playerAnimatorHandler.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quick.UpdateWeaponQuickSlotsUI(false, weaponItem);
                playerAnimatorHandler.anim.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;
            if (isLeft)
            {
                playerInventory.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quick.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                playerInventory.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quick.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorHandler.anim.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        
    }

    #region Handle Weapon DAmage Collider

    public void LoadLeftWeaponDamageCollider()
    {
        leftHandDamage = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        leftHandDamage.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        playerFXManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void LoadRightWeaponDamageCollider()
    {
        rightHandDamage = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamage.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        playerFXManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand)
        {
            rightHandDamage.EnableDamageCollider();
        }
        else if (playerManager.isUsingleftHand)
        {
            leftHandDamage.EnableDamageCollider();
        }
        
    }

    public void CloseHandDamageCollider()
    {
        rightHandDamage.DisableDamageCollider();
        leftHandDamage.DisableDamageCollider();
    }

    #endregion

    #region Handle Weapon's Stamina
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.LightAttackMultiple));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.HeavyAttackMultiple));
    }

    #endregion
}
