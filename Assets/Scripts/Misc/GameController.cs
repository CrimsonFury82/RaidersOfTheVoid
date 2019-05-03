using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script written by Aston Olsen

[Serializable]
public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    bool ultimateUsed;

    public Text apText, monstersText, turnText, gameoverText, ultimateText, warningText; //UI text

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    int AP, UltimateMaxCooldown, heroMaxHP, lootCounter, lootDrop = 3;

    public GameObject gameoverBackground, lootDropObj, buttonCanvas, warningObj, continueButtonObj; //menu objects that will be toggled off and on

    public Toggle menuToggle;

    Button endTurnButton;

    //Board zones for each group of cards
    public Transform enemyTransform, heroTransform, ultimateTransform, weaponTransform, armourTransform, lootChestTransform, backpackTransform;

    //Deck lists
    public List<WeaponData> weaponLoot1;
    public List<UltimateData> ultimateLoot1;
    public List<ArmourData> armourLoot1;
    public List<WeaponData> equippedWeapons; 
    public List<UltimateData> equippedUltimate;
    public List<ArmourData> equippedArmour;
    public List<HeroData> equippedHero;
    public List<CreatureData> currentAiDeck, aiDeck1, aiDeck2, aiDeck3;
    public List<WeaponData> allWeapons;
    public List<UltimateData> allUltimates;
    public List<ArmourData> allArmour;

    public List<int> Loottable;

    public List<string> textEquippedWeapons, textEquippedUltimate, textEquippedArmour, textBackpackWeapons, textBackpackUltimate, textBackpackArmour, textRemainingWeapons, textRemainingUltimates, textRemainingArmour;

    //Lists of card prefabs on the board
    public List<GameObject> equippedWeaponObj, equippedUltimateObj, equippedArmourObj, equippedCreaturesObj, equippedHeroObj, lootChest, backPackWeapons, backPackUltimates, backPackArmour;

    //card prefabs
    public CreatureCard creatureCardTemplate;
    public HeroCard heroCardTemplate;
    public ArmourCard armourCardTemplate;
    public UltimateCard ultimateCardTemplate;
    public WeaponCard weaponCardTemplate;
    
    //prefabs instances on the board
    HeroCard defHero;
    ArmourCard currentArmour;
    UltimateCard currentUltimate;

    //Item Dictionaries
    public Dictionary<string, WeaponData> weaponDictionary = new Dictionary<string, WeaponData>();
    public Dictionary<string, UltimateData> ultimateDictionary = new Dictionary<string, UltimateData>();
    public Dictionary<string, ArmourData> armourDictionary = new Dictionary<string, ArmourData>();

    public AnimationController animationController;

    public LevelController levelController;

    public WeaponData tempWeaponData; //currently selected weapon data
    public WeaponCard tempWeaponCard; //currently selected weapon card
    UltimateData tempUltimate = null; //currently selected ultimate

    float endTurnDelay = 1.75f;

    //Temp objects for top card of each deck
    BaseData TopDeck;
    CreatureData creatureTopDeck;
    HeroData heroTopDeck;
    ArmourData armourTopDeck;
    WeaponData weaponTopDeck;
    UltimateData ultimateTopDeck;
       
    void Start()
    {
        BeginGame();
    }

    void BeginGame() //function for starting the game.
    {
        GameObject controller = GameObject.FindGameObjectWithTag("MenuMusic");
        if (controller != null)
        {
            controller.GetComponent<MenuMusicController>().StopMusic();
        }
        ultimateUsed = false;
        if(LevelController.levelCounter == 0)
        {
            currentAiDeck = aiDeck1;
        }
        else if (LevelController.levelCounter == 1)
        {
            currentAiDeck = aiDeck2;
        }
        else if (LevelController.levelCounter == 2)
        {
            currentAiDeck = aiDeck3;
        }
        else
        {
            print("Level counter error");
        }
        CreatureShuffle(currentAiDeck);
        CreateDictionaries();
        LoadEquipment();
        DealHero();        
        DealCreatureHand();
        lootCounter = lootDrop;
        turnState = turn.Player1;
        if (turnState == turn.Player1) //checks if it is the Players's turn
        {
            for (int i = 0; i < equippedWeaponObj.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCard weapon = equippedWeaponObj[i].GetComponent<WeaponCard>();
                weapon.useButton.SetActive(true); //enables buttons on all weapon cards during player turn
            }
            if (currentUltimate.ultimateData.cooldown == 0) //Checks if Ultimte is ready
            {
                currentUltimate.useButton.SetActive(true); //enables button
            }
        }
        APReset();
    }

    void CreatureShuffle(List<CreatureData> deck)
    {
        for (int i = 0; i < 1000; i++) //shuffles by swapping two random cards and repeating process 1000 times
        {
            int rng1 = UnityEngine.Random.Range(0, deck.Count); //first randomly selected card number
            int rng2 = UnityEngine.Random.Range(0, deck.Count); //second randomly selected card number
            CreatureData tempcard = deck[rng1]; //tempcard to store copy of card 1
            deck[rng1] = deck[rng2]; //swaps card 2 into card 1's position in list
            deck[rng2] = tempcard; //swaps temp copy of card 1 into card 2's position in list
        }
    }

    public void Turns() //case switches for turn states
    {
        switch (turnState)
        {
            case turn.Player1:
                break;
            case turn.Player2:
                break;
            default:
                print("Default Turn triggered");
                break;
        }
    }

    public void Phases() //case switches for phase states
    {
        switch (turnPhase)
        {
            case phase.MainPhase:
                break;
            case phase.CombatPhase:
                break;
            default:
                print("Default Phase triggered");
                break;
        }
    }

    public void EndTurn() //function for end of turn
    {
        if (equippedCreaturesObj.Count == 0 && currentAiDeck.Count == 0) //checks if all enemies have been destroy for victory condition.
        {
            Victory();
            DropLoot();
            return;
        }

        turnPhase = phase.MainPhase;

        if (turnState == turn.Player1) //checks if currently Player 1's turn
        {
            turnState = turn.Player2;
            turnText.color = Color.red;
            turnText.text = "Wait - Enemy turn";
        }
        else if (turnState == turn.Player2) //checks if currently Player 2's turn
        {
            PlayerUpkeep();
        }
        TurnStart();
    }

    void PlayerUpkeep() //tasks to execute at start of each player's turn
    {
        turnState = turn.Player1;
        turnText.color = Color.green;
        turnText.text = "Go - Your turn";

        if (ultimateUsed == true)
        {
            currentUltimate.ultimateData.cooldown = UltimateMaxCooldown;
            ultimateUsed = false;
            ultimateText.text = "Ultimate charging"; //updates card UI text
            RelicUpdate();
        }
        else
        {
            currentUltimate.ultimateData.cooldown--; //updates cooldown
            if (currentUltimate.ultimateData.cooldown <= 0)
            {
                currentUltimate.ultimateData.cooldown = 0;
                currentUltimate.useButton.SetActive(true); //enables button
                ultimateText.color = Color.green; //changes font colour
                ultimateText.text = "Ultimate Ready"; //updates card UI text
            }
        }
        RelicUpdate();
        APReset();
    }

    public void RelicAttacked()
    {
        currentUltimate.useButton.SetActive(false); //disables button
        ultimateUsed = true;
        ultimateText.color = Color.red; //changes font colour
        ultimateText.text = "Ultimate Used"; //updates card UI text
    }
    
    void APReset() //resets action points at start of turn
    {
        AP = 10;
        APUpdate();
    }

    void APUpdate() //updates UI text
    {
        apText.text = "Action Points: " + AP.ToString();
    }

    void HeroHPUpdate() //updates UI text
    {
        defHero.hpText.text = defHero.heroCardData.hp.ToString();
    }

    void RelicUpdate() //updates UI text
    {
        currentUltimate.cooldownText.text = currentUltimate.ultimateData.cooldown.ToString(); //updates prefab text from scriptable object
    }

    void MonstersUpdate() //updates UI text
    {
        monstersText.text = "Enemies remaining: " + (currentAiDeck.Count + equippedCreaturesObj.Count).ToString(); 
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is the AI's turn
        {
            for (int i = 0; i < equippedWeaponObj.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCard weapon = equippedWeaponObj[i].GetComponent<WeaponCard>();
                weapon.useButton.SetActive(false); //disables buttons on all weapon cards during AI turn
            }

            currentUltimate.useButton.SetActive(false); //disables button

            if (equippedCreaturesObj.Count == 0) //checks if AI has creatures on the board
            {
                DealCreatureHand(); //Deals new row of creatures
                EndTurn(); //Ends turn
            }
            else
            {
                StartCoroutine(HeroAttackPhase()); //starts attack phase
            }
        }

        if (turnState == turn.Player1) //checks if it is the Players's turn
        {
            for (int i = 0; i < equippedWeaponObj.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCard weapon = equippedWeaponObj[i].GetComponent<WeaponCard>();
                weapon.useButton.SetActive(true); //enables buttons on all weapon cards during player turn
            }
        }
    }

    public void DealHero() //Deals hero cards at start of game
    {
        if (equippedHero.Count > 0)
        {
            heroTopDeck = equippedHero[0];
        }
        else
        {
            heroTopDeck = null;
        }
        HeroData card = Instantiate(heroTopDeck); //instantiates instance of scriptable object
        HeroCard tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card onto board
        tempCard.heroCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedHero.Remove(heroTopDeck);
        equippedHeroObj.Add(tempCard.gameObject); //adds card to live list
        defHero = equippedHeroObj[0].GetComponent<HeroCard>();
        defHero.heroCardData.armour = currentArmour.armourData.hp;
        heroMaxHP = defHero.heroCardData.hp;
        defHero.heroCardData.artSprite = currentArmour.armourData.artSprite;
    }

    public void DealArmour(Transform spawnTransform, ArmourData data, List<GameObject> objectList) //Deals hero cards at start of game
    {
        ArmourData card = Instantiate(data); //instantiates instance of scriptable object
        ArmourCard tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.armourData = card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedArmourObj.Add(tempCard.gameObject); //adds card to live list
        currentArmour = equippedArmourObj[0].GetComponent<ArmourCard>();
    }

    public void DealUltimate(Transform spawnTransform, UltimateData data, List<GameObject> objectList) //Deals hero cards at start of game
    {
        UltimateData card = Instantiate(data); //instantiates instance of scriptable object
        UltimateCard tempCard = Instantiate(ultimateCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.ultimateData = card; //assigns the instance of the scriptable object to the instance of the prefab
        objectList.Add(tempCard.gameObject); //adds card to live list
        currentUltimate = tempCard.GetComponent<UltimateCard>();
        UltimateMaxCooldown = currentUltimate.ultimateData.cooldown; //assigns cooldown for resetting ultimate after use
    }

    public void DealWeapon(Transform spawnTransform, WeaponData data, List<GameObject> objectList) //Deals one weapon card
    {
        WeaponData card = Instantiate(data); //instantiates instance of scriptable object
        WeaponCard tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.weaponData = card; //assigns the instance of the scriptable object to the instance of the prefab
        objectList.Add(tempCard.gameObject); //adds card to list
    }

    public void DealCreatureHand() //Deals multiple creature cards
    {
        StartCoroutine(DealerAnimation()); //starts animation
        lootCounter--;
        if(lootCounter == 0)
        {
            DropLoot();   
        }

        int dealMonsters = 4; //number of monsters to deal
        for (int i = 0; i < dealMonsters; i++) //loops number of times equal to dealMonsters variable
        {
            if (currentAiDeck.Count > 0)
            {
                DealCreature();
            }
        }
        MonstersUpdate();
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            SaveLootTable();
        }
    }

    public void DropLoot()
    {
        lootDropObj.SetActive(true); //enables menu
        lootCounter = lootDrop; //resets lootdrop counter

        Loottable.Clear();
        if (weaponLoot1.Count > 0)
        {
            Loottable.Add(0); //0 is the loot table index for Weapons
        }
        if (ultimateLoot1.Count > 0)
        {
            Loottable.Add(1); //1 is the loot table index for Ultimates
        }
        if (armourLoot1.Count > 0)
        {
            Loottable.Add(2); //2 is the loot table index for Armour
        }

        if(Loottable.Count == 0)
        {
            return;
        }
        else
        {
            int rng = UnityEngine.Random.Range(0, Loottable.Count); //randomly select a card
            int rng2 = Loottable[rng]; //picks a random number from the loottable

            if (rng2 == 0)
            {
                DealWeaponLoot(lootChestTransform, weaponLoot1, lootChest); //deals card to lootdrop zone
            }
            else if (rng2 == 1)
            {
                DealUltimateLoot(lootChestTransform, ultimateLoot1, lootChest); //deals card to lootdrop zone
            }
            else if (rng2 == 2)
            {
                DealArmourLoot(lootChestTransform, armourLoot1, lootChest); //deals card to lootdrop zone
            }
        }
    }

    public void DealWeaponLoot(Transform spawnTransform, List<WeaponData> dataList, List<GameObject> objectList) //Deals one weapon card
    {
        if (dataList.Count > 0)
        {
            int rng = UnityEngine.Random.Range(0, dataList.Count); //randomly select a card
            weaponTopDeck = dataList[rng];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCard tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.weaponData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponLoot1.Remove(weaponTopDeck); //removes from list
        objectList.Add(tempCard.gameObject); //adds card to list
        tempCard.equipButton.SetActive(true); //enables button
        tempCard.name = tempCard.weaponData.name.Replace("(Clone)", "").ToString();
    }

    public void DealUltimateLoot(Transform spawnTransform, List<UltimateData> dataList, List<GameObject> objectList) //Deals one weapon card
    {
        if (dataList.Count > 0)
        {
            int rng = UnityEngine.Random.Range(0, dataList.Count); //randomly select a card
            ultimateTopDeck = dataList[rng];
        }
        else
        {
            ultimateTopDeck = null;
        }
        UltimateData card = Instantiate(ultimateTopDeck); //instantiates instance of scriptable object
        UltimateCard tempCard = Instantiate(ultimateCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.ultimateData = card; //assigns the instance of the scriptable object to the instance of the prefab
        ultimateLoot1.Remove(ultimateTopDeck); //removes from list
        objectList.Add(tempCard.gameObject); //adds card to list
        tempCard.equipButton.SetActive(true); //enables button
        tempCard.name = tempCard.ultimateData.name.Replace("(Clone)", "").ToString();
    }

    public void DealArmourLoot(Transform spawnTransform, List<ArmourData> dataList, List<GameObject> objectList) //Deals one weapon card
    {
        if (dataList.Count > 0)
        {
            int rng = UnityEngine.Random.Range(0, dataList.Count); //randomly select a card
            armourTopDeck = dataList[rng];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCard tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.armourData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourLoot1.Remove(armourTopDeck); //removes from list
        objectList.Add(tempCard.gameObject); //adds card to list
        tempCard.equipButton.SetActive(true); //enables button
        tempCard.name = tempCard.armourData.name.Replace("(Clone)", "").ToString();
    }

    public void MoveWeaponBackpack(GameObject playedCard)
    {
        if (playedCard.transform.parent == lootChestTransform)
        {
            if ((backPackWeapons.Count + backPackUltimates.Count + backPackArmour.Count) >= 2)
            {
                WarningPopup("Backpack menu full");
            }
            else
            {
                playedCard.transform.SetParent(backpackTransform.transform, false); //moves the card to the zone
                backPackWeapons.Add(playedCard); //adds to list
                lootChest.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(lootChestTransform.transform, false); //moves the card to the zone
            backPackWeapons.Remove(playedCard); //removes from list
            lootChest.Add(playedCard); //adds to list
        }
    }

    public void MoveUltimateBackpack(GameObject playedCard)
    {
        if (playedCard.transform.parent == lootChestTransform)
        {
            if ((backPackWeapons.Count + backPackUltimates.Count + backPackArmour.Count) >= 2)
            {
                WarningPopup("Backpack menu full");
            }
            else
            {
                playedCard.transform.SetParent(backpackTransform.transform, false); //moves the card to the zone
                backPackUltimates.Add(playedCard); //adds to list
                lootChest.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(lootChestTransform.transform, false); //moves the card to the zone
            backPackUltimates.Remove(playedCard); //removes from list
            lootChest.Add(playedCard); //adds to list
        }
    }

    public void MoveArmourBackpack(GameObject playedCard)
    {
        if (playedCard.transform.parent == lootChestTransform)
        {
            if ((backPackWeapons.Count + backPackUltimates.Count + backPackArmour.Count) >= 2)
            {
                WarningPopup("Backpack menu full");
            }
            else
            {
                playedCard.transform.SetParent(backpackTransform.transform, false); //moves the card to the zone
                backPackArmour.Add(playedCard); //adds to list
                lootChest.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(lootChestTransform.transform, false); //moves the card to the zone
            backPackArmour.Remove(playedCard); //removes from list
            lootChest.Add(playedCard); //adds to list
        }
    }

    public void ClearLoot() //this function is called by the "Discard" button on the loot drop menu
    {
        foreach(GameObject item in lootChest)
        {
            Destroy(item);
        }
        lootChest.Clear();
    }

    public void DealCreature() //Deals one creature card
    {
        if (currentAiDeck.Count > 0)
        {
            creatureTopDeck = currentAiDeck[0];
        }
        else
        {
            creatureTopDeck = null;
        }

        CreatureData card = Instantiate(creatureTopDeck); //instantiates an instance of the carddata scriptable object
        CreatureCard dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(enemyTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        currentAiDeck.Remove(creatureTopDeck); //removes card from deck list
        equippedCreaturesObj.Add(dealtCard.gameObject);
    }

    public void ActivateRelic(UltimateData relicCardData, UltimateCard ultimateCard)
    {
        if(relicCardData.heal > 0)
        {
            HealRelic(relicCardData);
        }
        if (relicCardData.apBonus > 0)
        {
            APRelic(relicCardData);
        }
        RelicAttacked();
        ultimateCard.PlaySound1();
    }

    void HealRelic(UltimateData ultimateData)
    {
        if(defHero.heroCardData.hp + ultimateData.heal > heroMaxHP) //checks if healing will take hero past max HP
        {
            defHero.heroCardData.hp = heroMaxHP; //heals to max HP
        }
        else
        {
            defHero.heroCardData.hp += ultimateData.heal; //heals for heal amount on ultimate
        }
        HeroHPUpdate(); //updates card UI text
    }

    void APRelic(UltimateData relicCardData)
    {
        AP += relicCardData.apBonus; //updates action points
        APUpdate(); //updates UI text
    }

    public void WarningPopup(string passedText)
    {
        warningText.text = passedText;
        warningObj.SetActive(true);
    }

    public void WeaponTarget(WeaponData weaponData, WeaponCard weaponCard) //function for selecting weapon target
    {
        //print("standard targeting");
        if (AP - weaponData.ap < 0)
        {
            WarningPopup("Not enough Action Points");
        }
        else
        {
            weaponCard.PlaySound2();
            for (int i = 0; i < equippedCreaturesObj.Count; i++) //loop repeats for each creature on the board
            {
                CreatureCard attacker = equippedCreaturesObj[i].GetComponent<CreatureCard>();
                attacker.buttonObject.SetActive(false); //disables buttons on all creature card, so if the player changes target, it will disable out of range options.
            }

            tempWeaponData = weaponData; //assigns the weapon clicked as the tempweapon
            tempWeaponCard = weaponCard; //assigns the weapon clicked as the tempweapon
            int tempRange = weaponData.range;

            if (equippedCreaturesObj.Count < weaponData.range)
            {
                tempRange = equippedCreaturesObj.Count;
            }

            for (int i = 0; i < tempRange; i++) //loop repeats for each creature on the board
            {
                CreatureCard attacker = equippedCreaturesObj[i].GetComponent<CreatureCard>();
                attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
            }
        }
    }

    public void WeaponAttack(GameObject creature, CreatureData creatureCard, WeaponCard weaponCard)
    {
        if(tempUltimate == null)
        {
            AP -= tempWeaponData.ap; //updates AP remaining
            APUpdate(); //updates UI text
            creatureCard.hp -= tempWeaponData.dmg; //weapon deals dmg to creature
            weaponCard.PlaySound1();
        }
        else
        {
            creatureCard.hp -= tempUltimate.dmg; //ultimate deals dmg to creature
            tempUltimate = null;
        }
        CreatureCard defCreature = creature.GetComponent<CreatureCard>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCard deadCreature = creature.GetComponent<CreatureCard>();
            Destroy(deadCreature.gameObject); //destroys the creature
            equippedCreaturesObj.Remove(deadCreature.gameObject); //removes destroyed creature from attackers list
            MonstersUpdate(); //updates UI text
        }

        for (int i = 0; i < equippedCreaturesObj.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCard attacker = equippedCreaturesObj[i].GetComponent<CreatureCard>();
            attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
        }
    }

    public IEnumerator DealerAnimation()
    {
        float firstAnimationDelay = 0.7f;
        float secondAnimationDelay = 0.7f;
        GameObject.Find("MillerDealer").GetComponent<Animator>().SetBool("DealSequence", true); //Starts dealer animation
        yield return new WaitForSeconds(firstAnimationDelay); //waits for specified time delay
        defHero.PlaySound1(); //plays sound
        yield return new WaitForSeconds(secondAnimationDelay); //waits for specified time delay
        GameObject.Find("MillerDealer").GetComponent<Animator>().SetBool("DealSequence", false); //Stops dealer animation
    }

    public IEnumerator HeroAttackPhase()
    {
        turnPhase = phase.CombatPhase; //sets state of combat phase
        for (int i = 0; i < equippedCreaturesObj.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCard attackCreature = equippedCreaturesObj[i].GetComponent<CreatureCard>();
            StartCoroutine(animationController.AttackMove(equippedCreaturesObj[i], equippedHeroObj[0])); //starts coroutine for moving attacking card
            attackCreature.PlaySound(); //plays sound effect
            if (attackCreature.creatureCardData.dmg > currentArmour.armourData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attackCreature.creatureCardData.dmg - currentArmour.armourData.hp; //creature deals damage
            }
            HeroHPUpdate(); //updates UI text
            float combatDelay = 1.3f;

            if (i!=equippedCreaturesObj.Count-1)
            {
                yield return new WaitForSeconds(combatDelay);
            }
            else if (i == equippedCreaturesObj.Count - 1)
            {
                yield return null;
            }
        }

        if (defHero.heroCardData.hp <= 0) //checks if hero is dead
        {
            GameOver();
            CancelInvoke();
        }
        Invoke("EndTurn", endTurnDelay);
    }

    public void GameOver() //function for losing the game
    {
        gameoverText.text = "You lose";
        gameoverBackground.SetActive(true);
    }

    public void Victory() //function for winning the game
    {
        gameoverText.text = "You win";
        gameoverBackground.SetActive(true);
        buttonCanvas.SetActive(true);
        if(LevelController.levelCounter >= 2)
        {
            continueButtonObj.SetActive(false);
        }
    }

    public void NextLevel()
    {
        levelController.LoadNextLevel();
    }

    public void RestartGame()
    {
        LevelController.levelCounter = 0;
        SceneManager.LoadScene("GameScene");
    }

    public void SaveBackpack() //saves backpack lists and remaining lootdrop lists
    {
        //saves backpack items to list of strings

        textBackpackWeapons.Clear(); //clears list before saving
        foreach (GameObject weapon in backPackWeapons) 
        {
            WeaponCard tempWeapon = weapon.GetComponent<WeaponCard>();
            weaponLoot1.Remove(tempWeapon.weaponData); //removes from loot drop list
            textBackpackWeapons.Add(weapon.name.ToString()); //Converts data to string
        }

        textBackpackUltimate.Clear(); //clears list before saving
        foreach (GameObject ultimate in backPackUltimates) 
        {
            UltimateCard tempUltimate = ultimate.GetComponent<UltimateCard>();
            ultimateLoot1.Remove(tempUltimate.ultimateData); //removes from loot drop list
            textBackpackUltimate.Add(ultimate.name.ToString()); //Converts data to string
        }

        textBackpackArmour.Clear(); //clears list before saving
        foreach (GameObject armour in backPackArmour)
        {
            ArmourCard tempArmour = armour.GetComponent<ArmourCard>();
            armourLoot1.Remove(tempArmour.armourData); //removes from loot drop list
            textBackpackArmour.Add(armour.name.ToString()); //Converts data to string
        }

        //serializes list of strings to .dat files

        FileStream weaponFile = new FileStream("BackpackWeapons.dat", FileMode.Create);
        var bf = new BinaryFormatter();
        bf.Serialize(weaponFile, textBackpackWeapons);
        weaponFile.Close();

        FileStream ultimateFile = new FileStream("BackpackUltimates.dat", FileMode.Create);
        bf.Serialize(ultimateFile, textBackpackUltimate);
        ultimateFile.Close();

        FileStream armourFile = new FileStream("BackpackArmour.dat", FileMode.Create);
        bf.Serialize(armourFile, textBackpackArmour);
        armourFile.Close();

        //print("saved backpack");
        SaveLootTable();
        SceneManager.LoadScene("MenuScene");
    }

    void SaveLootTable()
    {
        //saves lootlist to list of strings

        textRemainingWeapons.Clear(); //clears list before saving
        foreach (WeaponData weapon in weaponLoot1)
        {
            textRemainingWeapons.Add(weapon.name.ToString()); //Converts data to string
        }

        textRemainingUltimates.Clear(); //clears list before saving
        foreach (UltimateData ultimate in ultimateLoot1)
        {
            textRemainingUltimates.Add(ultimate.name.ToString()); //Converts data to string
        }

        textRemainingArmour.Clear(); //clears list before saving
        foreach (ArmourData armour in armourLoot1)
        {
            textRemainingArmour.Add(armour.name.ToString()); //Converts ndata to string
        }

        FileStream weaponLootRemainingFile = new FileStream("WeaponsLoot.dat", FileMode.Create);
        var bf = new BinaryFormatter();
        bf.Serialize(weaponLootRemainingFile, textRemainingWeapons);
        weaponLootRemainingFile.Close();

        FileStream ultimateLootRemianingFile = new FileStream("UltimatesLoot.dat", FileMode.Create);
        bf.Serialize(ultimateLootRemianingFile, textRemainingUltimates);
        ultimateLootRemianingFile.Close();

        FileStream armourLootRemainingFile = new FileStream("ArmourLoot.dat", FileMode.Create);
        bf.Serialize(armourLootRemainingFile, textRemainingArmour);
        armourLootRemainingFile.Close();

        //print("Saved Loot Table");
    }

    void LoadEquipment()
    {
        //deserializes .data files to list of strings

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

        //uses dictionary to compare list of strings to data values and equips gear

        equippedWeaponObj.Clear(); //clears list
        foreach (string weaponName in textEquippedWeapons)
        {
            WeaponData weaponValue;
            if (weaponDictionary.TryGetValue(weaponName, out weaponValue))
            {
                DealWeapon(weaponTransform, weaponValue, equippedWeaponObj);
            }
        }

        equippedUltimateObj.Clear(); //clears list
        foreach (string ultimateName in textEquippedUltimate)
        {
            UltimateData ultimateValue;
            if (ultimateDictionary.TryGetValue(ultimateName, out ultimateValue))
            {
                DealUltimate(ultimateTransform, ultimateValue, equippedUltimateObj);
            }
        }

        equippedArmourObj.Clear(); //clears list
        foreach (string armourName in textEquippedArmour)
        {
            ArmourData armourValue;
            if (armourDictionary.TryGetValue(armourName, out armourValue))
            {
                DealArmour(armourTransform, armourValue, equippedArmourObj);
            }
        }
        //print("Loaded Equipment");
        LoadLootTable();
    }

    void LoadLootTable()
    {
        //deserializes .data files to list of strings
        using (FileStream weaponLootRemainingFile = File.Open("WeaponsLoot.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempWeapons = (List<string>)bf.Deserialize(weaponLootRemainingFile);
            textRemainingWeapons.Clear(); //clears list before loading
            for (int i = 0; i < tempWeapons.Count; i++)
            {
                textRemainingWeapons.Add(tempWeapons[i]);
            }
        }

        using (FileStream ultimateLootRemianingFile = File.Open("UltimatesLoot.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempUltimate = (List<string>)bf.Deserialize(ultimateLootRemianingFile);
            textRemainingUltimates.Clear(); //clears list before loading
            for (int i = 0; i < tempUltimate.Count; i++)
            {
                textRemainingUltimates.Add(tempUltimate[i]);
            }
        }

        using (FileStream armourLootRemainingFile = File.Open("ArmourLoot.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();
            List<string> tempArmour = (List<string>)bf.Deserialize(armourLootRemainingFile);
            textRemainingArmour.Clear(); //clears list before loading
            for (int i = 0; i < tempArmour.Count; i++)
            {
                textRemainingArmour.Add(tempArmour[i]);
            }
        }

        //uses dictionary to compare list of strings to data values and fills loot lists with data

        weaponLoot1.Clear(); //clears list
        foreach (string weaponName in textRemainingWeapons)
        {
            WeaponData weaponValue;
            if (weaponDictionary.TryGetValue(weaponName, out weaponValue))
            {
                weaponLoot1.Add(weaponValue);
            }
        }

        ultimateLoot1.Clear(); //clears list
        foreach (string ultimateName in textRemainingUltimates)
        {
            UltimateData ultimateValue;
            if (ultimateDictionary.TryGetValue(ultimateName, out ultimateValue))
            {
                ultimateLoot1.Add(ultimateValue);
            }
        }

        armourLoot1.Clear(); //clears list
        foreach (string armourName in textRemainingArmour)
        {
            ArmourData armourValue;
            if (armourDictionary.TryGetValue(armourName, out armourValue))
            {
                armourLoot1.Add(armourValue);
            }
        }
        //print("Loaded Loot Table");
    }

    void CreateDictionaries()
    {
        foreach (WeaponData weapon in allWeapons)
        {
            weaponDictionary.Add(weapon.name, weapon);
        }

        foreach (UltimateData ultimate in allUltimates)
        {
            ultimateDictionary.Add(ultimate.name, ultimate);
        }

        foreach (ArmourData armour in allArmour)
        {
            armourDictionary.Add(armour.name, armour);
        }
    }
}