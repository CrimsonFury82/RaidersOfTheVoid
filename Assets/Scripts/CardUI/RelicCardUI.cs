using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class RelicCardUI : BaseCardUI
{
    public GameController gameController;

    public RelicCardData relicCardData;

    public Button button;

    public GameObject buttonObject;

    public Text dmgText, healText, cooldownText;

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        UIRelicCard(dmgText, healText, cooldownText);
    }

    public void UIRelicCard(Text dmg, Text heal, Text cooldown)
    {
        relicCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = relicCardData.dmg.ToString(); //updates prefab with values from scriptable object
        healText.text = relicCardData.heal.ToString(); //updates prefab with values from scriptable object
        cooldownText.text = relicCardData.cooldown.ToString(); //updates prefab with values from scriptable object
    }

    public void UsedClickedGear() //calls targeting function in gamecontroller
    {
        gameController.SupportRelic(relicCardData);
    }
}