using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[ExecuteInEditMode]
public abstract class BaseCardPrefab : MonoBehaviour
{
    BaseCardData baseCardData;
    
    public Image artImage;

    public Text cardNameText, ability1Text, hpText;

    void Start()
    {
        baseCardData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage); //updates prefab UI text with values from scriptable object
    }
}