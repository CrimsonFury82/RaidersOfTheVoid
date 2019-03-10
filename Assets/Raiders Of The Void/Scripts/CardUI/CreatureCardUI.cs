﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreatureCardUI : BaseCardUI
{
    public CreatureCardData creatureCardData;

    GameController gameController;

    public Text dmgText;

    public Button button;

    public GameObject buttonObject;

    AudioSource audioSource;

    public AudioClip cardSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        UICreatureCard(dmgText);
    }

    public void UICreatureCard(Text dmg)
    {
        creatureCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
        dmg.text = creatureCardData.dmg.ToString(); //updates prefab with values from scriptable object
    }

    public void AttackClickedCreature() //plays the clicked card from your hand area to the battlezone
    {
        gameController.WeaponAttack(this.gameObject, creatureCardData, button);
    }
}