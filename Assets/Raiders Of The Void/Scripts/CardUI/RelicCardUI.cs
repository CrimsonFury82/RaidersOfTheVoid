using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicCardUI : BaseCardUI
{
    public RelicCardData relicCardData;

    public Button button;

    public Text dmgText, healText, cooldownText;

    private void Start()
    {
        UIRelicCard(dmgText, healText, cooldownText);
    }

    public void UIRelicCard(Text dmg, Text heal, Text cooldown)
    {
        relicCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = relicCardData.dmg.ToString(); //updates prefab with values from scriptable object
        healText.text = relicCardData.heal.ToString(); //updates prefab with values from scriptable object
        cooldownText.text = relicCardData.cooldown.ToString(); //updates prefab with values from scriptable object
    }
}