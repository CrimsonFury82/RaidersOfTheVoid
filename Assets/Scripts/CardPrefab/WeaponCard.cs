using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class WeaponCard : BaseCard
{
    public GameController gameController;

    public InventoryController inventoryController;

    AudioSource audioSource;

    public WeaponData weaponData;

    public WeaponCard weaponCard;

    public GameObject useButton, equipButton;

    public Text dmgText, ammoText, rangeText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        weaponCard = GetComponent<WeaponCard>();
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
        inventoryController = (InventoryController)FindObjectOfType(typeof(InventoryController)); //finds the inventorycontroller
        UIWeaponCard(dmgText, ammoText, rangeText);
    }

    public void UIWeaponCard(Text dmg, Text ap, Text range)
    {
        weaponData.BaseCardUpdate(cardNameText, ability1Text, hpText, artImage);
        dmgText.text = weaponData.dmg.ToString(); //updates prefab with values from scriptable object
        ammoText.text = weaponData.ap.ToString(); //updates prefab with values from scriptable object
        rangeText.text = weaponData.range.ToString(); //updates prefab with values from scriptable object
    }

    public void UsedClickedGear() //calls targeting function in gamecontroller
    {
        gameController.WeaponTarget(weaponData, weaponCard);
    }

    public void EquipInventory() //calls function in inventory controller
    {
        if (gameController != null)
        {
            gameController.MoveWeaponBackpack(this.gameObject); //puts in backpack in game scene
        }
        else
        {
            inventoryController.EquipWeapon(this.gameObject); //equips in inventory scene
        }
    }

    public void PlaySound1() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = weaponData.audio1;
        audioSource.PlayOneShot(attackSound);
    }

    public void PlaySound2() //plays audio clip once
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip attackSound = weaponData.audio2;
        audioSource.PlayOneShot(attackSound);
    }
}