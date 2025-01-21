using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion2 playerLocomotion;
        PlayerAnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion2>();
        animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponInventory.Add(weapon);
        playerManager.ItemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.ItemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.ItemInteractableGameObject.SetActive(true);
        Destroy(gameObject, 2.0f);
    }
}
