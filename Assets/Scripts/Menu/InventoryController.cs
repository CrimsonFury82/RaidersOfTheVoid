using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    //Board zones for each group of cards
    public Transform relicTransform, weaponTransform, armourTransform, relicInvTransform1, relicInvTransform2, weaponInvTransform1, weaponInvTransform2, armourInvTransform1, armourInvTransform2;

    //Spare inventory lists
    public List<WeaponCardData> weaponInv;
    public List<RelicCardData> relicInv;
    public List<ArmourCardData> armourInv;

    //Equipped inventory lists
    public List<GameObject> weaponSlots, relicSlot, armourSlot, liveRelics1, liveRelics2, liveWeapons1, liveWeapons2, liveArmour1, liveArmour2;

    //card prefabs
    public ArmourCardPrefab armourCardTemplate;
    public RelicCardPrefab relicCardTemplate;
    public WeaponCardPrefab weaponCardTemplate;

    //Temp objects for top card of each deck
    ArmourCardData armourTopDeck;
    WeaponCardData weaponTopDeck;
    RelicCardData relicTopDeck;

    RelicCardPrefab currentRelic;

    void Start()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MenuMusicController>().StartMusic();
        DealArmourInv();
        DealRelicInv();
        DealWeaponInv();
    }

    public void DealArmourInv() //Deals all cards in inventory
    {
        int loopSize = armourInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to variable
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
        ArmourCardPrefab tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourInvTransform1.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourInv.Remove(armourTopDeck); //removes the card from the deck
        liveArmour1.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipArmour(GameObject playedCard, ArmourCardData armourCardData)
    {
        if (playedCard.transform.parent == armourInvTransform1)
        {
            if (armourSlot.Count == 1)
            {
                print("Full");
            }
            else
            {
                playedCard.transform.SetParent(armourTransform.transform, false); //moves the card to the zone
                armourSlot.Add(playedCard); //adds the card to list
                liveArmour1.Remove(playedCard); //removes card from the list
            }
        }
        else
        {
            playedCard.transform.SetParent(armourInvTransform1.transform, false); //moves the card to the zone
            armourSlot.Remove(playedCard); //adds the card to list
            liveArmour1.Add(playedCard); //removes card from the list
        }
    }

    public void DealRelicInv() //Deals all cards in inventory
    {
        int loopSize = relicInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to variable
        {
            if (liveRelics1.Count >= 5 && relicInv.Count > 0)
            {
                DealRelic(relicInvTransform2, liveRelics2);
            }
            else if (weaponInv.Count > 0)
            {
                DealRelic(relicInvTransform1, liveRelics1);
            }
        }
    }

    public void DealRelic(Transform relicTransform, List<GameObject> relicObjectList) //Deals hero cards at start of game
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
        RelicCardPrefab tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        relicInv.Remove(relicTopDeck); //removes card from list
        relicObjectList.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipRelic(GameObject playedCard, RelicCardData relicCardData)
    {
        if (playedCard.transform.parent == relicInvTransform1)
        {
            if (relicSlot.Count == 1)
            {
                print("Full");
            }
            else
            {
                playedCard.transform.SetParent(relicTransform.transform, false); //moves the card to the zone
                relicSlot.Add(playedCard); //adds the card to list
                liveRelics1.Remove(playedCard); //removes card from the list
            }

        }
        else if (playedCard.transform.parent == relicInvTransform2)
        {
            if (relicSlot.Count == 1)
            {
                print("Full");
            }
            else
            {
                playedCard.transform.SetParent(relicTransform.transform, false); //moves the card to the zone
                relicSlot.Add(playedCard); //adds the card to list
                liveRelics2.Remove(playedCard); //removes card from the list
            }
        }
        else if (liveRelics1.Count >= 5)
        {
            playedCard.transform.SetParent(relicInvTransform2.transform, false); //moves the card to the zone
            relicSlot.Remove(playedCard); //adds the card to list
            liveRelics2.Add(playedCard); //removes card from the list
        }
        else
        {
            playedCard.transform.SetParent(relicInvTransform1.transform, false); //moves the card to the zone
            relicSlot.Remove(playedCard); //adds the card to list
            liveRelics1.Add(playedCard); //removes card from the list
        }
    }

    public void DealWeaponInv() //Deals all cards in inventory
    {
        int loopSize = weaponInv.Count;
        for (int i = 0; i < loopSize; i++) //loops number of times equal to list size
        {
            if (liveWeapons1.Count >= 5 && weaponInv.Count > 0)
            {
                DealWeapon(weaponInvTransform2, liveWeapons2);
            }
            else if (weaponInv.Count > 0)
            {
                DealWeapon(weaponInvTransform1, liveWeapons1);
            }
        }
    }

    public void DealWeapon(Transform weaponTransform, List <GameObject> weaponObjectList) //Deals one weapon card
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
        WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponTransform.transform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponInv.Remove(weaponTopDeck);
        weaponObjectList.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
    }

    public void EquipWeapon(GameObject playedCard, WeaponCardData weaponCardData)
    {
        if (playedCard.transform.parent == weaponInvTransform1)
        {
            if (weaponSlots.Count == 3)
            {
                print("Full");
            }
            else
            {
                playedCard.transform.SetParent(weaponTransform.transform, false); //moves the card to the zone
                weaponSlots.Add(playedCard); //adds the card to list
                liveWeapons1.Remove(playedCard); //removes card from the list
            }

        }
        else if (playedCard.transform.parent == weaponInvTransform2)
        {
            if (weaponSlots.Count == 3)
            {
                print("Full");
            }
            else
            {
                playedCard.transform.SetParent(weaponTransform.transform, false); //moves the card to the zone
                weaponSlots.Add(playedCard); //adds the card to list
                liveWeapons2.Remove(playedCard); //removes card from the list
            }
        }
        else if (liveWeapons1.Count >= 5)
        {
            playedCard.transform.SetParent(weaponInvTransform2.transform, false); //moves the card to the zone
            weaponSlots.Remove(playedCard); //adds the card to list
            liveWeapons2.Add(playedCard); //removes card from the list
        }
        else
        {
            playedCard.transform.SetParent(weaponInvTransform1.transform, false); //moves the card to the zone
            weaponSlots.Remove(playedCard); //adds the card to list
            liveWeapons1.Add(playedCard); //removes card from the list
        }
    }
}