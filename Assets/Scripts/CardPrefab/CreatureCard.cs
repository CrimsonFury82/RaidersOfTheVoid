using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[ExecuteInEditMode]
public class CreatureCard : BaseCard
{
    public CreatureData creatureData;

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
        creatureData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage); //updates card UI text
        dmg.text = creatureData.dmg.ToString(); //updates card UI text
    }

    public void AttackClickedCreature() //the previously selected weapon attacks the clicked creature
    {
        gameController.WeaponAttack(this.gameObject, creatureData, gameController.tempWeaponCard);
    }

    public void PlaySound() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = creatureData.audio1;
        audioSource.PlayOneShot(attackSound);
    }
}