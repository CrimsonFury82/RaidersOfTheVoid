using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourCardUI : BaseCardUI
{
    public ArmourCardData armourCardData;

    public Button button;

    private void Start()
    {
        //ArmourCardData data = Instantiate(armourCardData);
        UIArmourCard(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }

    public void UIArmourCard(Text cardName, Text ability1, Text ability2, Text hp, Image artImage)
    {
        armourCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }
}