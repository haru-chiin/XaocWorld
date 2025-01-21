using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    PlayerInventory playerInventory;
    WeaponSlotManager slotManager;
    UIManager uI;

    public Image icon;
    WeaponItem weaponItem;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        slotManager = FindObjectOfType<WeaponSlotManager>();
        uI = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        weaponItem = newItem;
        if (newItem)
        {
            icon.sprite = weaponItem.itemIcon; 
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        
    }

    public void ClearInventory()
    {
        weaponItem = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
        
    }

    public void EquipThisItem()
    {
        if (uI.rightHandSlot01Selected)
        {
            playerInventory.weaponInventory.Add(playerInventory.weaponInRightHandSlots[0]);
            playerInventory.weaponInRightHandSlots[0] = weaponItem;
            playerInventory.weaponInventory.Remove(weaponItem);
        }
        else if (uI.rightHandSlot02Selected)
        {
            playerInventory.weaponInventory.Add(playerInventory.weaponInRightHandSlots[1]);
            playerInventory.weaponInRightHandSlots[1] = weaponItem;
            playerInventory.weaponInventory.Remove(weaponItem);
        }
        else if (uI.leftHandSlot01Selected)
        {
            playerInventory.weaponInventory.Add(playerInventory.weaponInLefttHandSlots[0]);
            playerInventory.weaponInLefttHandSlots[0] = weaponItem;
            playerInventory.weaponInventory.Remove(weaponItem);
        }
        else if(uI.leftHandSlot02Selected)
        {
            playerInventory.weaponInventory.Add(playerInventory.weaponInLefttHandSlots[1]);
            playerInventory.weaponInLefttHandSlots[1] = weaponItem;
            playerInventory.weaponInventory.Remove(weaponItem);
        }
        else
        {
            return;
        }

        playerInventory.rightWeapon = playerInventory.weaponInRightHandSlots[playerInventory.currentRightWeaponIndex];
        playerInventory.leftWeapon = playerInventory.weaponInLefttHandSlots[playerInventory.currentLeftWeaponIndex];

        slotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        slotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

        uI.equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
        uI.resetAllSelectedSlots();
    }
}
