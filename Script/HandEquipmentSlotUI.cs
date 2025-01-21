using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandEquipmentSlotUI : MonoBehaviour
{
    UIManager uI;

    public Image icon;
    WeaponItem weaponItem;

    public bool rightHandSlot01;
    public bool rightHandSlot02;
    public bool leftHandSlot01;
    public bool leftHandSlot02;

    private void Awake()
    {
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

    public void SelectThisSlot()
    {
        if (rightHandSlot01)
        {
            uI.rightHandSlot01Selected = true;
        }
        else if (rightHandSlot02)
        {
            uI.rightHandSlot02Selected = true;
        }
        else if (leftHandSlot01)
        {
            uI.leftHandSlot01Selected = true;
        }
        else
        {
            uI.leftHandSlot02Selected = true;
        }
    }
}
