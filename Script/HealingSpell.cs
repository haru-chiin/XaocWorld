using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttempToCastSpell(PlayerAnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        base.AttempToCastSpell(animatorHandler, playerStats);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
        animatorHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(animatorHandler, playerStats);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
        playerStats.healPlayer(healAmount);
    }
}
