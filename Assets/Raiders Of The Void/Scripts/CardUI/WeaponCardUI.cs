using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCardUI : BaseCardUI
{
    public WeaponCardData weaponCardData;

    public Button button;

    public Text dmgText, apText;

    private void Start()
    {
        UIWeaponCard(dmgText, apText);
    }

    public void UIWeaponCard(Text dmg, Text ap)
    {
        weaponCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
        dmgText.text = weaponCardData.dmg.ToString(); //updates prefab with values from scriptable object
        apText.text = weaponCardData.ap.ToString(); //updates prefab with values from scriptable object
    }
}