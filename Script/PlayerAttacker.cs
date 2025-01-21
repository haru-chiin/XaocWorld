using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorHandler animatorHandler;
    InputHandler inputHandler;
    PlayerInventory playerInventory;
    WeaponSlotManager slotManager;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerEffectManager playerEffectManager;

    [Header("Attack Animations")]
    string oh_LA_01 = "OH_LA1";
    string oh_LA_02 = "OH_LA2";
    string oh_LA_03 = "OH_LA3";
    string oh_LA_04 = "OH_LA4";

    string oh_HA_01 = "OH_HA1";
    string oh_HA_02 = "OH_HA2";
    string oh_HA_03 = "OH_HA3";
    string oh_HA_04 = "OH_HA4";

    string th_LA_01 = "TH_LH01";
    string th_LA_02 = "TH_LH02";
    string th_LA_03 = "TH_LH03";

    string th_HA_01 = "TH_HH_01";

    string weaponArt = "Weapon_Art";

    LayerMask backStabLayer = 1 << 13;
    LayerMask riposteLayer = 1 << 14;

    public string lastAttack;
    private int lightComboStep;
    private int heavyComboStep;
    private int thComboStep;
    private bool isHeavyCombo;

    private void Awake()
    {
        animatorHandler = GetComponent<PlayerAnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
        inputHandler = GetComponent<InputHandler>();
        slotManager = GetComponent<WeaponSlotManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerEffectManager = GetComponent<PlayerEffectManager>();
        lightComboStep = 0;
        heavyComboStep = 0;
        thComboStep = 0;
/*        hthComboStep = 0;*/
        isHeavyCombo = false;
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStats.CurrentStamina <= 0)
            return;

        if (inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool("canDoCombo", false);

            if (inputHandler.twoHandedFlag)
            {
                thComboStep++;
                switch (thComboStep)
                {
                    case 1:
                        animatorHandler.PlayTargetAnimation(th_LA_01, true);
                        lastAttack = th_LA_01;
                        break;
                    case 2:
                        if (lastAttack == th_LA_01)
                        {
                            animatorHandler.PlayTargetAnimation(th_LA_02, true);
                            lastAttack = th_LA_02;
                        }
                        break;
                    case 3:
                        if (lastAttack == th_LA_02)
                        {
                            animatorHandler.PlayTargetAnimation(th_LA_03, true);
                            lastAttack = th_LA_03;
                        }
                        break;
                    default:
                        ResetCombo();
                        break;
                }
            }
            else if(inputHandler.twoHandedFlag){
                if (isHeavyCombo)
                {
                    animatorHandler.PlayTargetAnimation(th_HA_01, true);
                    lastAttack = th_HA_01;
                }

            }
            else
            {
                if (isHeavyCombo)
                {
                    // Heavy attack combo
                    heavyComboStep++;
                    switch (heavyComboStep)
                    {
                        case 1:
                            animatorHandler.PlayTargetAnimation(oh_HA_01, true);
                            lastAttack = oh_HA_01;
                            break;
                        case 2:
                            if (lastAttack == oh_HA_01)
                            {
                                animatorHandler.PlayTargetAnimation(oh_HA_02, true);
                                lastAttack = oh_HA_02;
                            }
                            break;
                        case 3:
                            if (lastAttack == oh_HA_02)
                            {
                                animatorHandler.PlayTargetAnimation(oh_HA_03, true);
                                lastAttack = oh_HA_03;
                            }
                            break;
                        case 4:
                            if (lastAttack == oh_HA_03)
                            {
                                animatorHandler.PlayTargetAnimation(oh_HA_04, true);
                                lastAttack = oh_HA_04;
                            }
                            break;
                        // Add more cases for additional heavy combo steps
                        default:
                            ResetCombo();
                            break;
                    }
                }
                else
                {
                    // Light attack combo
                    lightComboStep++;
                    switch (lightComboStep)
                    {
                        case 1:
                            animatorHandler.PlayTargetAnimation(oh_LA_01, true);
                            lastAttack = oh_LA_01;
                            break;
                        case 2:
                            if (lastAttack == oh_LA_01)
                            {
                                animatorHandler.PlayTargetAnimation(oh_LA_02, true);
                                lastAttack = oh_LA_02;
                            }
                            break;
                        case 3:
                            if (lastAttack == oh_LA_02)
                            {
                                animatorHandler.PlayTargetAnimation(oh_LA_03, true);
                                lastAttack = oh_LA_03;
                            }
                            break;
                        case 4:
                            if (lastAttack == oh_LA_03)
                            {
                                animatorHandler.PlayTargetAnimation(oh_LA_04, true);
                                lastAttack = oh_LA_04;
                            }
                            break;
                        // Add more cases for additional light combo steps
                        default:
                            ResetCombo();
                            break;
                    }
                }
            }

            
        }
        else
        {
            ResetCombo();
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.CurrentStamina <= 0)
            return;

        slotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandedFlag)
        {
            animatorHandler.PlayTargetAnimation(th_LA_01, true);
            lastAttack = th_LA_01;
        }
        else
        {
            isHeavyCombo = false;
            animatorHandler.PlayTargetAnimation(oh_LA_01, true);
            lastAttack = oh_LA_01;
            lightComboStep = 1; // Start the light combo sequence
        }


    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.CurrentStamina <= 0)
            return;

        slotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandedFlag)
        {
            isHeavyCombo = true;
            animatorHandler.PlayTargetAnimation(th_HA_01, true);
            lastAttack = th_HA_01;
            heavyComboStep = 1;
        }
        else
        {
            
            isHeavyCombo = true;
            animatorHandler.PlayTargetAnimation(oh_HA_01, true);
            lastAttack = oh_HA_01;
            heavyComboStep = 1; // Start the heavy combo sequence
        }

    }

    private void ResetCombo()
    {
        lightComboStep = 0;
        heavyComboStep = 0;
        thComboStep = 0;
        animatorHandler.anim.SetBool("canDoCombo", false);
    }

    #region input actions
    public void HandleLAAction()
    {
        if (playerInventory.rightWeapon.weaponType == WeaponType.StraighSword || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLAMeleeAction();
        }else if (playerInventory.rightWeapon.weaponType == WeaponType.Caster || playerInventory.rightWeapon.weaponType == WeaponType.Pyro || playerInventory.rightWeapon.weaponType == WeaponType.Faith)
        {
            PerformLAMagicAction(playerInventory.rightWeapon);
        }
    }

    private void PerformCWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {
            
        }
        else
        {
            animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.Weapon_Art, true);
        }
    }

    public void HandleQAction()
    {
        PerformQBlockingAction();
    }

    public void HandleParryAction()
    {
        if (playerInventory.leftWeapon.weaponType == WeaponType.Shield || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformCWeaponArt(inputHandler.twoHandedFlag);
        }
        else if (playerInventory.leftWeapon.weaponType == WeaponType.StraighSword)
        {

        }
    }

    #endregion
    #region Attack Actions
    private void PerformLAMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;

            animatorHandler.anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }

        playerEffectManager.PlayWeaponFX(false);
    }

    private void PerformLAMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
            return;

        if (weapon.weaponType == WeaponType.StraighSword)
        {
            if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                if(playerStats.currentManaPoint >= playerInventory.currentSpell.manaPointCost){
                    playerInventory.currentSpell.AttempToCastSpell(animatorHandler, playerStats);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Shrug", true);
                }
                
            }
        }
    }

    private void SuccessfullyCastSpell()
    {
        playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        
    }

    #endregion

    public void AttemptBackStabOrRiposte()
    {
        if (playerStats.CurrentStamina <= 0)
            return;

        RaycastHit hit;
        if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = slotManager.rightHandDamage;

            if(enemyCharacterManager != null)
            {
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                animatorHandler.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
            }
        }
        else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = slotManager.rightHandDamage;
            

            if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposeCollider.criticalDamagerStandPosition.position;
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                animatorHandler.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
            }

           
        }
    }

    #region Defense Action
    private void PerformQBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        animatorHandler.PlayTargetAnimation("Block", false);
    }

    #endregion
}
