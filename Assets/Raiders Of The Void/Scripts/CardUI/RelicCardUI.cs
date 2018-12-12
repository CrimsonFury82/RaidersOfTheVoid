using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicCardUI : BaseCardUI
{
    public RelicCardData relicCardData;

    public Button button;

    private void Start()
    {
        //RelicCardData data = Instantiate(relicCardData);
        UIRelicCard(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }

    public void UIRelicCard(Text cardName, Text ability1, Text ability2, Text hp, Image artImage)
    {
        relicCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
    }
}