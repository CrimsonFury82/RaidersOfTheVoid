using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class WeaponCardPrefab : BaseCardPrefab
{
    public GameController gameController;

    public InventoryController inventoryController;

    public WeaponCardData weaponCardData;

    public GameObject useButton, equipButton;

    public Text dmgText, apText, rangeText;

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        inventoryController = (InventoryController)FindObjectOfType(typeof(InventoryController)); //finds the inventorycontroller
        UIWeaponCard(dmgText, apText, rangeText);
    }

    public void UIWeaponCard(Text dmg, Text ap, Text range)
    {
        weaponCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = weaponCardData.dmg.ToString(); //updates prefab with values from scriptable object
        apText.text = weaponCardData.ap.ToString(); //updates prefab with values from scriptable object
        rangeText.text = weaponCardData.range.ToString(); //updates prefab with values from scriptable object
    }

    public void UsedClickedGear() //calls targeting function in gamecontroller
    {
        gameController.WeaponTarget(weaponCardData);
    }

    public void EquipClickedGear() //calls function in inventory controller
    {
        inventoryController.EquipWeapon(this.gameObject, weaponCardData, equipButton);
    }
}