using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("animator replacer")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle";

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Weapon Art")]
    public string Weapon_Art;

    [Header("Damage")]
    public int baseDamage = 25;
    public int criticalDamageMultiplier = 4;

    [Header("Stamina Cost")]
    public int baseStamina;
    public float LightAttackMultiple;
    public float HeavyAttackMultiple;


}
