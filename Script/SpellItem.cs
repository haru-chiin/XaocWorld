using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell cost")]
    public int manaPointCost;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttempToCastSpell(PlayerAnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        Debug.Log("You cast a spell");
    }

    public virtual void SuccessfullyCastSpell(PlayerAnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        Debug.Log("you success to cast a spell");
        playerStats.DeductManaPoint(manaPointCost);
    }

}
