using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class RelicCardPrefab : BaseCardPrefab
{
    public GameController gameController;

    public InventoryController inventoryController;

    public RelicCardData relicCardData;

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
        relicCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = relicCardData.dmg.ToString(); //updates prefab with values from scriptable object
        healText.text = relicCardData.heal.ToString(); //updates prefab with values from scriptable object
        cooldownText.text = relicCardData.cooldown.ToString(); //updates prefab with values from scriptable object
    }

    public void UsedClickedGear() //calls function in gamecontroller
    {
        gameController.SupportRelic(relicCardData);
    }

    public void EquipClickedGear() //calls function in inventorycontroller
    {
        inventoryController.EquipRelic(this.gameObject, relicCardData, equipButton);
    }
}