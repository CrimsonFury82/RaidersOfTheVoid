using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    //Board zones for each group of cards
    public Transform relicTransform1, weaponTransform1, weaponTransform2, weaponTransform3, armourTransform1, relicInvTransform, weaponInvTransform, armourInvTransform;

    //Spare inventory lists
    public List<WeaponCardData> weaponInv;
    public List<RelicCardData> relicInv;
    public List<ArmourCardData> armourInv;

    //Equipped inventory lists
    public List<GameObject> weaponSlots, relicSlot, armourSlot, liveRelic, liveWeapons, liveArmour;

    //card prefabs
    public ArmourCardUI armourCardTemplate;
    public RelicCardUI relicCardTemplate;
    public WeaponCardUI weaponCardTemplate;

    //Temp objects for top card of each deck
    ArmourCardData armourTopDeck;
    WeaponCardData weaponTopDeck;
    RelicCardData relicTopDeck;

    RelicCardUI currentRelic;

    void Start()
    {
        DealArmourInv();
        DealRelicInv();
        DealWeaponInv();
    }

    public void DealArmourInv() //Deals all cards in inventory
    {
        int loop = armourInv.Count;
        for (int i = 0; i < loop; i++) //loops number of times equal to variable
        {
            if (armourInv.Count > 0)
            {
                DealArmour();
            }
        }
    }

    public void DealArmour() //Deals hero cards at start of game
    {
        if (armourInv.Count > 0)
        {
            armourTopDeck = armourInv[0];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourCardData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCardUI tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourInvTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourInv.Remove(armourTopDeck); //removes the card from the deck
        liveArmour.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipArmour(GameObject playedCard, ArmourCardData armourCardData, GameObject equipButton)
    {
        if (armourSlot.Count == 1)
        {
            print("Full");
        }
        else
        {
            equipButton.SetActive(false); //disables the button
            playedCard.transform.SetParent(armourTransform1.transform, false); //moves the card to the zone
            armourSlot.Add(playedCard); //adds the card to list
            liveArmour.Remove(playedCard); //removes card from the list
        }
    }

    public void DealRelicInv() //Deals all cards in inventory
    {
        int loop = relicInv.Count;
        for (int i = 0; i < loop; i++) //loops number of times equal to variable
        {
            if (relicInv.Count > 0)
            {
                DealRelic();
            }
        }
    }

    public void DealRelic() //Deals hero cards at start of game
    {
        if (relicInv.Count > 0)
        {
            relicTopDeck = relicInv[0];
        }
        else
        {
            relicTopDeck = null;
        }
        RelicCardData card = Instantiate(relicTopDeck); //instantiates instance of scriptable object
        RelicCardUI tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicInvTransform.transform, false); //moves card onto board
        tempCard.relicCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        relicInv.Remove(relicTopDeck); //removes card from list
        liveRelic.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipRelic(GameObject playedCard, RelicCardData relicCardData, GameObject equipButton)
    {
        if (relicSlot.Count == 1)
        {
            print("Full");
        }
        else
        {
            equipButton.SetActive(false); //disables the button
            playedCard.transform.SetParent(relicTransform1.transform, false); //moves the card to the zone
            relicSlot.Add(playedCard); //adds the card to list
            liveRelic.Remove(playedCard); //removes card from the list
        }
    }

    public void DealWeaponInv() //Deals all cards in inventory
    {
        int loop = weaponInv.Count;
        for (int i = 0; i < loop; i++) //loops number of times equal to list size
        {
            if (weaponInv.Count > 0)
            {
                DealWeapon();
            }
        }
    }

    public void DealWeapon() //Deals one weapon card
    {
        if (weaponInv.Count > 0)
        {
            weaponTopDeck = weaponInv[0];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponCardData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCardUI tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponInvTransform.transform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponInv.Remove(weaponTopDeck);
        liveWeapons.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipWeapon(GameObject playedCard, WeaponCardData weaponCardData, GameObject equipButton)
    {
        if(weaponSlots.Count == 3)
        {
            print("Full");
        }
        else
        {
            equipButton.SetActive(false); //disables the button
            playedCard.transform.SetParent(weaponTransform1.transform, false); //moves the card to the zone
            weaponSlots.Add(playedCard); //adds the card to list
            liveWeapons.Remove(playedCard); //removes card from the list
        }
    }
}