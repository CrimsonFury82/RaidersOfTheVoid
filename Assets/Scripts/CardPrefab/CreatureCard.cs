﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[ExecuteInEditMode]
public class CreatureCard : BaseCard
{
    public CreatureData creatureCardData;

    GameController gameController;

    public Text dmgText;

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
        creatureCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage); //updates card UI text
        dmg.text = creatureCardData.dmg.ToString(); //updates card UI text
    }

    public void AttackClickedCreature() //the previously selected weapon attacks the clicked creature
    {
        gameController.WeaponAttack(this.gameObject, creatureCardData);
    }

    public void PlaySound() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = creatureCardData.audio1;
        audioSource.PlayOneShot(attackSound);
    }
}