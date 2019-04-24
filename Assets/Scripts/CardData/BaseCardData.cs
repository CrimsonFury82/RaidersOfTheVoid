using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[ExecuteInEditMode]
public class BaseCardData : ScriptableObject
{
    public string cardName, ability1;
    
    public int hp;

    public Sprite artSprite;

    public AudioClip audio1;

    public virtual void BaseCardUpdate(Text cardNameText, Text ability1Text, Text hpText, Image artImage)
    {
        cardNameText.text = cardName; //updates card UI text
        ability1Text.text = ability1; //updates card UI text
        hpText.text = hp.ToString(); //updates card UI text
        artImage.sprite = artSprite; //updates card art image
    }
}