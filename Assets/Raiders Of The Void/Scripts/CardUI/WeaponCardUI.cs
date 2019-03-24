using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class WeaponCardUI : BaseCardUI
{
    public GameController gameController;

    public WeaponCardData weaponCardData;

    public Button button;

    public Text dmgText, apText, rangeText;

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
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
        gameController.WeaponTarget(weaponCardData, button);
    }
}