using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot leftHandSlot;

    EnemyEffectManager enemyEffectManager;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    private void Awake()
    {
        enemyEffectManager = GetComponentInParent<EnemyEffectManager>();
        LoadWeaponHolderSlot();
    }

    private void Start()
    {
        LoadWeaponOnBothHands();
    }

    private void LoadWeaponHolderSlot()
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
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponDamageCollider(false);
        }
    }

    public void LoadWeaponOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }
        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponDamageCollider(bool isLeft)
    {
        if (isLeft)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            enemyEffectManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            enemyEffectManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    #region Handle Weapon's Stamina
    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {
        
    }

    #endregion

    public void EnableCombo()
    {
        /*anim.SetBool("canDoCombo", true);*/
    }

    public void DisableCombo()
    {
        /*anim.SetBool("canDoCombo", false);*/
    }
}
