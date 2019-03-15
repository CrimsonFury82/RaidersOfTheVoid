using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BaseCardData : ScriptableObject
{
    public string cardName, ability1;
    
    public int hp;

    public Sprite artSprite;

    public AudioClip audio1;

    public virtual void BaseCardUpdate(Text cardNameText, Text ability1Text, Text hpText, Image artImage)
    {
        cardNameText.text = cardName;
        ability1Text.text = ability1;
        hpText.text = hp.ToString();
        artImage.sprite = artSprite;
    }
}