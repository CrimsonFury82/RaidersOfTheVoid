using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class ArmourCard : BaseCard
{
    public GameController gameController;

    public InventoryController inventoryController;

    public ArmourData armourData;

    public GameObject equipButton;

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        inventoryController = (InventoryController)FindObjectOfType(typeof(InventoryController)); //finds the inventorycontroller
        UIArmourCard();
    }

    public void UIArmourCard()
    {
        armourData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
    }

    public void EquipClickedGear() //calls function in inventorycontroller
    {

        if (gameController != null)
        {
            gameController.MoveArmourBackpack(this.gameObject); //puts in backpack in game scene
        }
        else
        {
            inventoryController.EquipArmour(this.gameObject); //equips in inventory scene
        }
    }
}