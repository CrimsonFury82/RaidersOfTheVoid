using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCardUI : BaseCardUI
{
    public WeaponCardData weaponCardData;

    public Button button;

    private void Start()
    {
        //WeaponCardData data = Instantiate(weaponCardData);
        UIWeaponCard(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }

    public void UIWeaponCard(Text cardName, Text ability1, Text ability2, Text hp, Image artImage)
    {
        weaponCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }
}