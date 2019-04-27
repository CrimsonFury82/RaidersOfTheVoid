using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class ArmourCard : BaseCard
{
    public InventoryController inventoryController;

    public ArmourData armourCardData;

    public GameObject equipButton;

    private void Start()
    {
        inventoryController = (InventoryController)FindObjectOfType(typeof(InventoryController)); //finds the inventorycontroller
        UIArmourCard();
    }

    public void UIArmourCard()
    {
        armourCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
    }

    public void EquipClickedGear() //calls function in inventorycontroller
    {
        inventoryController.EquipArmour(this.gameObject, armourCardData);
    }
}