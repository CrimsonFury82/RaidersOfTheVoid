using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureCardUI : BaseCardUI
{
    public CreatureCardData creatureCardData;

    public Text attackText;

    public Button button;

    private void Start()
    {
        //CreatureCardData data = Instantiate(creatureCardData);
        UICreatureCard(cardNameText, ability1Text, ability2Text, hpText, artImage, attackText);
    }

    public void UICreatureCard(Text cardName, Text ability1, Text ability2, Text hp, Image artImage, Text attack)
    {
        creatureCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
        attack.text = creatureCardData.attack.ToString();
    }
}