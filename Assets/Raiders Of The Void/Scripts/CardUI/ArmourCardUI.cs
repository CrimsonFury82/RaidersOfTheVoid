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
        UIArmourCard();
    }

    public void UIArmourCard()
    {
        armourCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
    }
}