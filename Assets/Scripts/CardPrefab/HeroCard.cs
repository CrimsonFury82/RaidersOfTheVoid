using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class HeroCard : BaseCard
{
    public HeroData heroCardData;

    public Text armourText;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UIHeroCard(armourText);
    }

    public void UIHeroCard(Text armour)
    {
       heroCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
       armourText.text = heroCardData.armour.ToString(); //updates prefab with values from scriptable object
    }

    public void PlaySound1() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = heroCardData.audio1;
        audioSource.PlayOneShot(attackSound);
    }
}