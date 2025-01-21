using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Potion")]
public class PotionItem : ConsumableItem
{
    [Header("Potion Type")]
    public bool HealPotion;
    public bool ManaPotion;

    [Header("Recovery Amount")]
    public int healthRecoveryAmount;
    public int manaPointRecoveryAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFx;

    public override void AttemptToConsumeItem(PlayerAnimatorHandler playerAnimatorHandler, WeaponSlotManager weaponSlotManager, PlayerEffectManager playerEffectManager)
    {
        base.AttemptToConsumeItem(playerAnimatorHandler, weaponSlotManager, playerEffectManager);
        GameObject Potion = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectManager.currentParticelFX = recoveryFx;
        playerEffectManager.amountToBeHealed = healthRecoveryAmount;
        playerEffectManager.instantiatedFXModel = Potion;
        weaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
    }
}
