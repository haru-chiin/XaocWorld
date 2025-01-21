using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelUpInteractable : Interactable
{
    public override void Interact(PlayerManager playerManager)
    {
        playerManager.uIManager.levelupWindow.SetActive(true);
    }
}
