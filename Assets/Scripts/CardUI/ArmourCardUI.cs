using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class ArmourCardUI : BaseCardUI
{
    public ArmourCardData armourCardData;

    public Button button;

    private void Start()
    {
        UIArmourCard();
    }

    public void UIArmourCard()
    {
        armourCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
    }
}