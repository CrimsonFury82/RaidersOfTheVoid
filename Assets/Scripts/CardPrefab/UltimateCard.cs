using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class UltimateCard : BaseCard
{
    public GameController gameController;

    public InventoryController inventoryController;

    public UltimateData ultimateData;

    public GameObject useButton, equipButton;

    public Text dmgText, healText, cooldownText;

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        inventoryController = (InventoryController)FindObjectOfType(typeof(InventoryController)); //finds the inventorycontroller
        UIRelicCard(dmgText, healText, cooldownText);
    }

    public void UIRelicCard(Text dmg, Text heal, Text cooldown)
    {
        ultimateData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = ultimateData.dmg.ToString(); //updates prefab with values from scriptable object
        healText.text = ultimateData.heal.ToString(); //updates prefab with values from scriptable object
        cooldownText.text = ultimateData.cooldown.ToString(); //updates prefab with values from scriptable object
        if(gameController != null)
        {
            gameController.ultimateText.color = Color.red;
            gameController.ultimateText.text = "Ultimate charging"; //updates prefab with values from scriptable object
        }
    }

    public void UsedClickedGear() //calls function in gamecontroller
    {
        gameController.ActivateRelic(ultimateData);
    }

    public void EquipClickedGear() //calls function in inventorycontroller
    {
        if (gameController != null)
        {
            gameController.EquipUltimate(this.gameObject); //puts in backpack in game scene
        }
        else
        {
            inventoryController.EquipUltimate(this.gameObject); //equips in inventory scene
        }
    }
}