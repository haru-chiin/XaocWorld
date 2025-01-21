using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public ConsumableItem currentConsumable;
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;



    public WeaponItem[] weaponInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponInLefttHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    public List<WeaponItem> weaponInventory;

    private void Awake()
    {
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void Start()
    {
        rightWeapon = weaponInRightHandSlots[0];
        leftWeapon = weaponInLefttHandSlots[0];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeRightHandWeapon()
    {
        if (currentRightWeaponIndex <  weaponInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
        else
            currentRightWeaponIndex = -1;

        if (currentRightWeaponIndex == 0 &&  weaponInRightHandSlots[0] != null)
        {
            rightWeapon =  weaponInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot( weaponInRightHandSlots[currentRightWeaponIndex], false);
        }
        else if (currentRightWeaponIndex == 0 &&  weaponInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 &&  weaponInRightHandSlots[1] != null)
        {
            rightWeapon =  weaponInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot( weaponInRightHandSlots[currentRightWeaponIndex], false);
        }

        if (currentRightWeaponIndex == -1)
        {
            rightWeapon = weaponSlotManager.unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(weaponSlotManager.unarmedWeapon, false);
        }
    }



    public void ChangeLeftHandWeapon()
    {
        if (currentLeftWeaponIndex < weaponInLefttHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
        else
        {
            currentLeftWeaponIndex = -1;
        }

        if (currentLeftWeaponIndex == 0 && weaponInLefttHandSlots[0] != null)
        {
            leftWeapon = weaponInLefttHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponInLefttHandSlots[currentLeftWeaponIndex], true);
        }
        else if (currentLeftWeaponIndex == 0 && weaponInLefttHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
        else if (currentLeftWeaponIndex == 1 && weaponInLefttHandSlots[1] != null)
        {
            leftWeapon = weaponInLefttHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponInLefttHandSlots[currentLeftWeaponIndex], true);
        }
        // Add more else if blocks if you have more weapon slots

        if (currentLeftWeaponIndex == -1)
        {
            leftWeapon = weaponSlotManager.unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(weaponSlotManager.unarmedWeapon, true);
        }
    }


}
