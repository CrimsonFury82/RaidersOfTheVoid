using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class InventoryController : MonoBehaviour
{
    //Board zones for each group of cards
    public Transform relicTransform, weaponTransform, armourTransform, relicInvTransform, weaponInvTransform, armourInvTransform;

    //Inventory lists
    public List<WeaponCardData> weaponInv;
    public List<RelicCardData> relicInv;
    public List<ArmourCardData> armourInv;

    //Equipped items lists
    public List<GameObject> weaponSlots, relicSlot, armourSlot, invRelics, invWeapons, invArmour;

    //Card prefabs
    public ArmourCardPrefab armourCardTemplate;
    public RelicCardPrefab relicCardTemplate;
    public WeaponCardPrefab weaponCardTemplate;

    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("MenuMusic");
        if (controller != null)
        {
            controller.GetComponent<MenuMusicController>().StartMusic(); //plays menu music
        }

        DealArmourInv();
        DealRelicInv();
        DealWeaponInv();
    }

    public void DealArmourInv() //Deals all cards in inventory
    {
        int loopSize = armourInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to loopsize
        {
            if (armourInv.Count > 0)
            {
                DealArmour();
            }
        }
    }

    public void DealArmour() //Deals hero cards at start of game
    {
        ArmourCardData armourTopDeck;
        if (armourInv.Count > 0)
        {
            armourTopDeck = armourInv[0];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourCardData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCardPrefab tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourInvTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourInv.Remove(armourTopDeck);  //removes from list
        invArmour.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipArmour(GameObject playedCard, ArmourCardData armourCardData)
    {
        if (playedCard.transform.parent == armourInvTransform)
        {
            if (armourSlot.Count == 1)
            {
                print("Armour slot full");
            }
            else
            {
                playedCard.transform.SetParent(armourTransform.transform, false); //moves the card to the zone
                armourSlot.Add(playedCard); //adds to list
                invArmour.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(armourInvTransform.transform, false); //moves the card to the zone
            armourSlot.Remove(playedCard); //removes from list
            invArmour.Add(playedCard); //adds to list
        }
    }

    public void DealRelicInv() //Deals all cards in inventory
    {
        int loopSize = relicInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to loopsize
        {
            if (relicInv.Count > 0)
            {
                DealRelic(relicInvTransform, invRelics);
            }
        }
    }

    public void DealRelic(Transform relicTransform, List<GameObject> relicObjectList) //Deals hero cards at start of game
    {
        RelicCardData relicTopDeck;
        if (relicInv.Count > 0)
        {
            relicTopDeck = relicInv[0];
        }
        else
        {
            relicTopDeck = null;
        }
        RelicCardData card = Instantiate(relicTopDeck); //instantiates instance of scriptable object
        RelicCardPrefab tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        relicInv.Remove(relicTopDeck);  //removes from list
        relicObjectList.Add(tempCard.gameObject); //adds to list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipRelic(GameObject playedCard, RelicCardData relicCardData)
    {
        if (playedCard.transform.parent == relicInvTransform)
        {
            if (relicSlot.Count == 1)
            {
                print("Ultimate slot full");
            }
            else
            {
                playedCard.transform.SetParent(relicTransform.transform, false); //moves the card to the zone
                relicSlot.Add(playedCard); //adds to list
                invRelics.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(relicInvTransform.transform, false); //moves the card to the zone
            relicSlot.Remove(playedCard); //removes from list
            invRelics.Add(playedCard); //adds to list
        }
    }

    public void DealWeaponInv() //Deals all cards in inventory
    {
        int loopSize = weaponInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to loopsize
        {
            if(weaponInv.Count > 0)
            {
                DealWeapon(weaponInvTransform, invWeapons);
            }
        }
    }

    public void DealWeapon(Transform weaponTransform, List <GameObject> weaponObjectList) //Deals one weapon card
    {
        WeaponCardData weaponTopDeck;
        if (weaponInv.Count > 0)
        {
            weaponTopDeck = weaponInv[0];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponCardData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponTransform.transform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponInv.Remove(weaponTopDeck); //removes from list
        weaponObjectList.Add(tempCard.gameObject); //adds to list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipWeapon(GameObject playedCard, WeaponCardData weaponCardData)
    {
        if (playedCard.transform.parent == weaponInvTransform)
        {
            if (weaponSlots.Count == 3)
            {
                print("Weapon slots full");
            }
            else
            {
                playedCard.transform.SetParent(weaponTransform.transform, false); //moves the card to the zone
                weaponSlots.Add(playedCard); //adds to list
                invWeapons.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(weaponInvTransform.transform, false); //moves the card to the zone
            weaponSlots.Remove(playedCard); //removes from list
            invWeapons.Add(playedCard); //adds to list
        }
    }
}