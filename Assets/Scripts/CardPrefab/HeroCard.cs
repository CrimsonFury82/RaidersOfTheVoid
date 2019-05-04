using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class HeroCard : BaseCard
{
    public HeroData heroData;

    public Text armourText;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UIHeroCard(armourText);
    }

    public void UIHeroCard(Text armour)
    {
       heroData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
       armourText.text = heroData.armour.ToString(); //updates prefab with values from scriptable object
    }

    public void PlaySound1() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = heroData.audio1;
        audioSource.PlayOneShot(attackSound);
    }
}