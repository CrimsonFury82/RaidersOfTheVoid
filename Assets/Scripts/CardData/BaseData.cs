using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[ExecuteInEditMode]
public class BaseData : ScriptableObject
{
    [SerializeField]
    public string cardName, ability1;

    [SerializeField]
    public int hp;

    [SerializeField]
    public Sprite artSprite;

    [SerializeField]
    public AudioClip audio1, audio2;

    [SerializeField]
    public virtual void BaseCardUpdate(Text cardNameText, Text ability1Text, Text hpText, Image artImage)
    {
        cardNameText.text = cardName; //updates card UI text
        ability1Text.text = ability1; //updates card UI text
        hpText.text = hp.ToString(); //updates card UI text
        artImage.sprite = artSprite; //updates card art image
    }
}