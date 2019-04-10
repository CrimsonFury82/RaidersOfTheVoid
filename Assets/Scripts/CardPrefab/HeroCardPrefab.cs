using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class HeroCardPrefab : BaseCardPrefab
{
    public HeroCardData heroCardData;

    public Text armourText;

    void Start()
    {
        UIHeroCard(armourText);
    }

    public void UIHeroCard(Text armour)
    {
       heroCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
       armourText.text = heroCardData.armour.ToString(); //updates prefab with values from scriptable object
    }
}