using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : CharacterFXManager
{
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;
    public GameObject currentParticelFX;
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HalPlayerFromEffect()
    {
        playerStats.healPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticelFX, playerStats.transform);
        Destroy(instantiatedFXModel, 1f);
        weaponSlotManager.LoadBothWeaponOnSlot();
    }
}
