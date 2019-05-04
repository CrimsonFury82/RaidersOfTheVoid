using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[ExecuteInEditMode]
public abstract class BaseCard : MonoBehaviour
{
    BaseData baseData;
    
    public Image artImage;

    public Text cardNameText, ability1Text, hpText;

    void Start()
    {
        baseData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage); //updates prefab UI text with values from scriptable object
    }
}