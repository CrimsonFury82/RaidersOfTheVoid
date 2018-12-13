using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreatureCardUI : BaseCardUI
{
    public CreatureCardData creatureCardData;

    GameController gameController;

    public Text attackText;

    public Button button;

    AudioSource audioSource;

    public AudioClip cardSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        UICreatureCard(cardNameText, ability1Text, ability2Text, hpText, artImage, attackText);
    }

    public void UICreatureCard(Text cardName, Text ability1, Text ability2, Text hp, Image artImage, Text attack)
    {
        creatureCardData.BaseCardUpdate(cardNameText, ability1Text, ability2Text, hpText, artImage);
        attack.text = creatureCardData.attack.ToString();
    }
}