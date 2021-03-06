﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script written by Aston Olsen

public class InventoryController : MonoBehaviour
{
    //Board zones for each group of cards
    public Transform relicTransform, weaponTransform, armourTransform, ultimateInvTransform, weaponInvTransform, armourInvTransform;

    //Inventory lists
    public List<WeaponData> allWeapons, startingWeapons;
    public List<UltimateData> allUltimates, startingUltimates;
    public List<ArmourData> allArmour, StartingArmour;
    public List<string> textEquippedWeapons, textEquippedUltimate, textEquippedArmour, textInventoryWeapons, textInventoryUltimates, textInventoryArmour, emptyList;

    public Text warningText;

    //Equipped items lists
    public List<GameObject> equippedWeaponObj, equippedUltimateObj, equippedArmourObj, invWeapons, invUltimates, invArmour;

    public GameObject warningObj;

    //Card prefabs
    public ArmourCard armourCardTemplate;
    public UltimateCard ultimateCardTemplate;
    public WeaponCard weaponCardTemplate;

    //Item Dictionaries
    public Dictionary<string, WeaponData> weaponDataDictionary = new Dictionary<string, WeaponData>();
    public Dictionary<string, UltimateData> ultimateDataDictionary = new Dictionary<string, UltimateData>();
    public Dictionary<string, ArmourData> armourDataDictionary = new Dictionary<string, ArmourData>();
    public Dictionary<string, GameObject> weaponObjDictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ultimateObjDictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> armourObjDictionary = new Dictionary<string, GameObject>();

    void Start()
    {
        //TextAsset dataAsset = (TextAsset)Resources.Load("WeaponLoot");
        //byte[] data = dataAsset.bytes;

        GameObject controller = GameObject.FindGameObjectWithTag("MenuMusic");
        if (controller != null)
        {
            controller.GetComponent<MenuMusicController>().StartMusic(); //plays menu music
        }
        CreateDataDictionaries();
        LoadInventory();
        DealArmourInv();
        DealUltimateInv();
        DealWeaponInv();
        CreateObjDictonaries();
        LoadEquipped();
    }

    public void WarningPopup(string passedText)
    {
        warningText.text = passedText;
        warningObj.SetActive(true);
    }

    public void DealArmourInv() //Deals all cards in inventory
    {
        foreach (ArmourData armour in StartingArmour)
        {
            DealArmour(armourInvTransform, invArmour, armour);
        }
    }

    public void DealArmour(Transform armourTransform, List<GameObject> armourObjectList, ArmourData armour) //Deals one card
    {
        ArmourData card = Instantiate(armour); //instantiates instance of scriptable object
        ArmourCard tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourInvTransform.transform, false); //moves card onto board
        tempCard.armourData = card; //assigns the instance of the scriptable object to the instance of the prefab
        invArmour.Add(tempCard.gameObject); //adds card to live list
        tempCard.equipButton.SetActive(true); //enables the button
        tempCard.name = tempCard.armourData.name.Replace("(Clone)", "").ToString();
    }

    public void EquipArmour(GameObject playedCard)
    {
        if (playedCard.transform.parent == armourInvTransform)
        {
            if (equippedArmourObj.Count >= 1)
            {
                WarningPopup("Armour slot full");
            }
            else
            {
                playedCard.transform.SetParent(armourTransform.transform, false); //moves the card to the zone
                equippedArmourObj.Add(playedCard); //adds to list
                invArmour.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(armourInvTransform.transform, false); //moves the card to the zone
            equippedArmourObj.Remove(playedCard); //removes from list
            invArmour.Add(playedCard); //adds to list
        }
    }

    public void DealUltimateInv() //Deals all cards in inventory
    {
        foreach (UltimateData ultimate in startingUltimates)
        {
            DealUltimate(ultimateInvTransform, invUltimates, ultimate);
        }
    }

    public void DealUltimate(Transform ultimateTransform, List<GameObject> relicObjectList, UltimateData ultimate) //Deals one card
    {
        UltimateData card = Instantiate(ultimate); //instantiates instance of scriptable object
        UltimateCard tempCard = Instantiate(ultimateCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(ultimateTransform.transform, false); //moves card onto board
        tempCard.ultimateData = card; //assigns the instance of the scriptable object to the instance of the prefab
        relicObjectList.Add(tempCard.gameObject); //adds to list
        tempCard.equipButton.SetActive(true); //enables the button
        tempCard.name = tempCard.ultimateData.name.Replace("(Clone)", "").ToString();
    }

    public void EquipUltimate(GameObject playedCard)
    {
        if (playedCard.transform.parent == ultimateInvTransform)
        {
            if (equippedUltimateObj.Count >= 1)
            {
                WarningPopup("Ultimate slot full");
            }
            else
            {
                playedCard.transform.SetParent(relicTransform.transform, false); //moves the card to the zone
                equippedUltimateObj.Add(playedCard); //adds to list
                invUltimates.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(ultimateInvTransform.transform, false); //moves the card to the zone
            equippedUltimateObj.Remove(playedCard); //removes from list
            invUltimates.Add(playedCard); //adds to list
        }
    }

    public void DealWeaponInv() //Deals all cards in inventory
    {
        foreach(WeaponData weapon in startingWeapons)
        {
            DealWeapon(weaponInvTransform, invWeapons, weapon);
        }
    }

    public void DealWeapon(Transform weaponTransform, List <GameObject> weaponObjectList, WeaponData weapon) //Deals one card
    {
        WeaponData card = Instantiate(weapon); //instantiates instance of scriptable object
        WeaponCard tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponTransform.transform, false); //moves card onto board
        tempCard.weaponData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponObjectList.Add(tempCard.gameObject); //adds to list
        tempCard.equipButton.SetActive(true); //enables the button
        tempCard.name = tempCard.weaponData.name.Replace("(Clone)", "").ToString();
    }

    public void EquipWeapon(GameObject playedCard)
    {
        if (playedCard.transform.parent == weaponInvTransform)
        {
            if (equippedWeaponObj.Count >= 3)
            {
                WarningPopup("Weapon slots full");
            }
            else
            {
                playedCard.transform.SetParent(weaponTransform.transform, false); //moves the card to the zone
                equippedWeaponObj.Add(playedCard); //adds to list
                invWeapons.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(weaponInvTransform.transform, false); //moves the card to the zone
            equippedWeaponObj.Remove(playedCard); //removes from list
            invWeapons.Add(playedCard); //adds to list
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            SaveInventory();
        }
    }

    public void ClearBackpack() //saves empty string to .dat files to clear them
    {
        emptyList.Add("");
        FileStream weaponFile = new FileStream("BackpackWeapons.dat", FileMode.Create);
        var bf = new BinaryFormatter();
        bf.Serialize(weaponFile, emptyList);
        weaponFile.Close();

        FileStream ultimateFile = new FileStream("BackpackUltimates.dat", FileMode.Create);
        bf.Serialize(ultimateFile, emptyList);
        ultimateFile.Close();

        FileStream armourFile = new FileStream("BackpackArmour.dat", FileMode.Create);
        bf.Serialize(armourFile, emptyList);
        armourFile.Close();
        //print("Backpack cleared");
    }

    public void SaveInventory()
    {
        //serializes list of strings to .dat files

        FileStream invWeaponFile = new FileStream("InvWeapons.dat", FileMode.Create);
        var bf = new BinaryFormatter();
        bf.Serialize(invWeaponFile, textInventoryWeapons);
        invWeaponFile.Close();

        FileStream invUltimateFile = new FileStream("InvUltimate.dat", FileMode.Create);
        bf.Serialize(invUltimateFile, textInventoryUltimates);
        invUltimateFile.Close();

        FileStream invArmourFile = new FileStream("InvArmour.dat", FileMode.Create);
        bf.Serialize(invArmourFile, textInventoryArmour);
        invArmourFile.Close();
        //print("Saved inventory");
        ClearBackpack();
    }

    public void SaveEquippedAndExit()
    {
        if ((equippedWeaponObj.Count + equippedArmourObj.Count + equippedUltimateObj.Count) != 5)
        {
            WarningPopup("Fill all slots before saving");
            return;
        }
        else
        {
            //saves list of gameobject names as a list of strings

            textEquippedWeapons.Clear(); //clears list before saving
            foreach (GameObject weapon in equippedWeaponObj) //loops through equipped weapons
            {
                textEquippedWeapons.Add(weapon.name.ToString()); //Converts weapondata to string
            }

            textEquippedUltimate.Clear(); //clears list before saving
            foreach (GameObject ultimate in equippedUltimateObj) //loops through equipped weapons
            {
                textEquippedUltimate.Add(ultimate.name.ToString()); //Converts weapondata to string
            }

            textEquippedArmour.Clear(); //clears list before saving
            foreach (GameObject armour in equippedArmourObj) //loops through equipped weapons
            {
                textEquippedArmour.Add(armour.name.ToString()); //Converts weapondata to string
            }

            //serializes list of strings to .dat files

            FileStream weaponFile = new FileStream("EquippedWeapons.dat", FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(weaponFile, textEquippedWeapons);
            weaponFile.Close();

            FileStream ultimateFile = new FileStream("EquippedUltimate.dat", FileMode.Create);
            bf.Serialize(ultimateFile, textEquippedUltimate);
            ultimateFile.Close();

            FileStream armourFile = new FileStream("EquippedArmour.dat", FileMode.Create);
            bf.Serialize(armourFile, textEquippedArmour);
            armourFile.Close();

            //print("saved equipped");
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void LoadBackPack()
    {
        //deserializes backpack items from .dat files to list of strings

        using (FileStream weaponFile = File.Open("BackpackWeapons.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempWeapons = (List<string>)bf.Deserialize(weaponFile);
            for (int i = 0; i < tempWeapons.Count; i++)
            {
                textInventoryWeapons.Add(tempWeapons[i]);
            }
        }

        using (FileStream ultimateFile = File.Open("BackpackUltimates.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempUltimate = (List<string>)bf.Deserialize(ultimateFile);
            for (int i = 0; i < tempUltimate.Count; i++)
            {
                textInventoryUltimates.Add(tempUltimate[i]);
            }
        }

        using (FileStream armourFile = File.Open("BackpackArmour.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempArmour = (List<string>)bf.Deserialize(armourFile);
            for (int i = 0; i < tempArmour.Count; i++)
            {
                textInventoryArmour.Add(tempArmour[i]);
            }
        }
        //print("loaded backpack");
        SaveInventory();
    }

    public void LoadInventory()
    {
        //deserializes inventory items

        using (FileStream invWeaponFile = File.Open("InvWeapons.dat", FileMode.Open))
        {
            textInventoryWeapons.Clear(); //Clears list
            var bf = new BinaryFormatter();
            List<string> tempWeapons = (List<string>)bf.Deserialize(invWeaponFile);
            for (int i = 0; i < tempWeapons.Count; i++)
            {
                textInventoryWeapons.Add(tempWeapons[i]);
            }
        }

        using (FileStream invUltimateFile = File.Open("InvUltimate.dat", FileMode.Open))
        {
            textInventoryUltimates.Clear(); //Clears list
            var bf = new BinaryFormatter();
            List<string> tempUltimate = (List<string>)bf.Deserialize(invUltimateFile);
            for (int i = 0; i < tempUltimate.Count; i++)
            {
                textInventoryUltimates.Add(tempUltimate[i]);
            }
        }

        using (FileStream invArmourFile = File.Open("InvArmour.dat", FileMode.Open))
        {
            textInventoryArmour.Clear(); //Clears list
            var bf = new BinaryFormatter();
            List<string> tempArmour = (List<string>)bf.Deserialize(invArmourFile);
            for (int i = 0; i < tempArmour.Count; i++)
            {
                textInventoryArmour.Add(tempArmour[i]);
            }
        }
        
        LoadBackPack();

        //uses dictionary to compare list of strings with data and populate lists of data

        foreach (string weaponName in textInventoryWeapons)
        {
            WeaponData weaponValue;
            if (weaponDataDictionary.TryGetValue(weaponName, out weaponValue))
            {
                DealWeapon(weaponInvTransform, invWeapons, weaponValue);
            }
        }

        foreach (string ultimateName in textInventoryUltimates)
        {
            UltimateData UltimateValue;
            if (ultimateDataDictionary.TryGetValue(ultimateName, out UltimateValue))
            {
                DealUltimate(ultimateInvTransform, invUltimates, UltimateValue);
            }
        }

        foreach (string armourName in textInventoryArmour)
        {
            ArmourData armourValue;
            if (armourDataDictionary.TryGetValue(armourName, out armourValue))
            {
                DealArmour(armourInvTransform, invArmour, armourValue);
            }
        }
        //print("Loaded inventory");
    }

    public void LoadEquipped()
    {
        using (FileStream weaponFile = File.Open("EquippedWeapons.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempWeapons = (List<string>)bf.Deserialize(weaponFile);
            textEquippedWeapons.Clear(); //clears list before loading
            for (int i = 0; i < tempWeapons.Count; i++)
            {
                textEquippedWeapons.Add(tempWeapons[i]);
            }
        }

        using (FileStream ultimateFile = File.Open("EquippedUltimate.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempUltimate = (List<string>)bf.Deserialize(ultimateFile);
            textEquippedUltimate.Clear(); //clears list before loading
            for (int i = 0; i < tempUltimate.Count; i++)
            {
                textEquippedUltimate.Add(tempUltimate[i]);
            }
        }

        using (FileStream armourFile = File.Open("EquippedArmour.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempArmour = (List<string>)bf.Deserialize(armourFile);
            textEquippedArmour.Clear(); //clears list before loading
            for (int i = 0; i < tempArmour.Count; i++)
            {
                textEquippedArmour.Add(tempArmour[i]);
            }
        }

        foreach (string weaponName in textEquippedWeapons)
        {
            GameObject weaponValue;
            if (weaponObjDictionary.TryGetValue(weaponName, out weaponValue))
            {
                EquipWeapon(weaponValue);
            }
        }

        foreach (string ultimateName in textEquippedUltimate)
        {
            GameObject UltimateValue;
            if (ultimateObjDictionary.TryGetValue(ultimateName, out UltimateValue))
            {
                EquipUltimate(UltimateValue);
            }
        }

        foreach (string armourName in textEquippedArmour)
        {
            GameObject armourValue;
            if (armourObjDictionary.TryGetValue(armourName, out armourValue))
            {
                EquipArmour(armourValue);
            }
        }
        //print("Loaded Equipped");
    }

    void CreateDataDictionaries()
    {
        foreach (WeaponData weapon in allWeapons)
        {
            weaponDataDictionary.Add(weapon.name, weapon);
        }

        foreach (UltimateData ultimate in allUltimates)
        {
            ultimateDataDictionary.Add(ultimate.name, ultimate);
        }

        foreach (ArmourData armour in allArmour)
        {
            armourDataDictionary.Add(armour.name, armour);
        }
    }

    void CreateObjDictonaries()
    {
        foreach (GameObject weapon in invWeapons)
        {
            weaponObjDictionary.Add(weapon.name, weapon);
        }

        foreach (GameObject ultimate in invUltimates)
        {
            ultimateObjDictionary.Add(ultimate.name, ultimate);
        }

        foreach (GameObject armour in invArmour)
        {
            armourObjDictionary.Add(armour.name, armour);
        }
    }
}