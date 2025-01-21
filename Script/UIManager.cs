using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerManager playerManager;
    /*public PlayerInventory playerInventory;*/
    public EquipmentWindowUI equipmentWindowUI;

    [Header("HUD")]
    public Text soulCount;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject SelectectWindow;
    public GameObject equipmentWindow; 
    public GameObject weaponInventoryWindow;
    public GameObject levelupWindow;

    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventoryPrefabs;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
        /*equipmentWindowUI = FindAnyObjectByType<EquipmentWindowUI>();*/
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerManager.playerInventory);
        soulCount.text = playerManager.playerStats.currentSoul.ToString();    
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerManager.playerInventory.weaponInventory.Count)
            {
                if (weaponInventorySlots.Length < playerManager.playerInventory.weaponInventory.Count)
                {
                    Instantiate(weaponInventoryPrefabs, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                }
                weaponInventorySlots[i].AddItem(playerManager.playerInventory.weaponInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventory();
            }
        }
        #endregion
    }

    public void OpenSelectedWindow()
    {
        SelectectWindow.SetActive(true);
    }

    public void ClosedSelectedWindow()
    {
        SelectectWindow.SetActive(false);
    }

    public void closeAllInventoryWindows()
    {
        resetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentWindow.SetActive(false);
    }

    public void resetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }
}
