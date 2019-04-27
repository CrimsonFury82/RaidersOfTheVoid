using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Script written by Aston Olsen

[Serializable]
public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states
    public enum lootNum {Tier1, Tier2, Tier3}; //List of states
    public enum lootTy {Weapon, Ultimate, Armour } //List of states

    bool relicUsed;

    public Text apText, monstersText, turnText, gameoverText, ultimateText; //UI text

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    public lootNum lootTier; //State for Loot drop tier

    public lootTy lootType; //State for Loot drop type

    int AP, RelicMaxCooldown, heroMaxHP, lootCounter, lootDrop = 3;

    public GameObject gameoverBackground, lootDropObj, buttonCanvas; //menu objects that will be toggled off and on

    public Toggle menuToggle;

    Button endTurnButton;

    //Board zones for each group of cards
    public Transform enemyTransform, heroTransform, relicTransform, weaponTransform, armourTransform, lootChestTransform, backpackTransform;

    //Deck lists
    public List<WeaponData> weaponLoot1; //, weaponLoot2, weaponLoot3;
    public List<UltimateData> relicLoot1; //, relicLoot2, relicLoot3;
    public List<ArmourData> armourLoot1; //, armourLoot2, armourLoot3;
    public List<WeaponData> equippedWeapons; 
    public List<UltimateData> equippedRelic;
    public List<ArmourData> equippedArmour;
    public List<HeroData> equippedHero;
    public List<CreatureData> aiDeck1, aiDeck2, aiDeck3, currentAiDeck;
    public List<DeckData> equipmentDeck;
    public List<string> textEquipmentDeck;

    //Lists of card prefabs on the board
    public List<GameObject> equippedWeaponObj, equippedRelicObj, equippedArmourObj, equippedCreaturesObj, equippedHeroObj, lootChest, backPack;

    //card prefabs
    public CreatureCard creatureCardTemplate;
    public HeroCard heroCardTemplate;
    public ArmourCard armourCardTemplate;
    public UltimateCard relicCardTemplate;
    public WeaponCard weaponCardTemplate;
    
    //prefabs instances on the board
    HeroCard defHero;
    ArmourCard currentArmour;
    UltimateCard currentRelic;

    public AnimationController animationController;

    public LevelController levelController;

    WeaponData tempWeapon; //currently selected weapon
    UltimateData tempRelic = null; //current selected attack relic

    float endTurnDelay = 1.75f;

    //Temp objects for top card of each deck
    BaseData TopDeck;
    CreatureData creatureTopDeck;
    HeroData heroTopDeck;
    ArmourData armourTopDeck;
    WeaponData weaponTopDeck;
    UltimateData relicTopDeck;
       
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
        relicUsed = false;
        currentAiDeck = aiDeck1;
        CreatureShuffle(currentAiDeck);
        InventoryLoad();
        DealArmour();
        DealHero();        
        DealRelic();
        DealWeaponHand();
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
            if (currentRelic.relicCardData.cooldown == 0) //Checks if Ultimte is ready
            {
                currentRelic.useButton.SetActive(true); //enables button
            }
        }
        APReset();
    }

    void InventoryLoad()
    {
        //foreach (WeaponData deck in equipmentDeck[0])
        //{
        //    //equippedWeapons.Add(equipmentDeck[0]);
        //    print("weapon test");
        //}

        //equippedWeapons.Add(equipmentDeck[0]);
        //print("loaded inventory");
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

    public void LootTier() //case switches for phase states
    {
        switch (lootTier)
        {
            case lootNum.Tier1:
                break;
            case lootNum.Tier2:
                break;
            case lootNum.Tier3:
                break;
            default:
                print("Default loot tier");
                break;
        }
    }

    public void LootType() //case switches for phase states
    {
        switch (lootType)
        {
            case lootTy.Weapon:
                break;
            case lootTy.Ultimate:
                break;
            case lootTy.Armour:
                break;
            default:
                print("Default loot type");
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

        if (relicUsed == true)
        {
            currentRelic.relicCardData.cooldown = RelicMaxCooldown;
            relicUsed = false;
            ultimateText.text = "Ultimate charging"; //updates card UI text
            RelicUpdate();
        }
        else
        {
            currentRelic.relicCardData.cooldown--; //updates cooldown
            if (currentRelic.relicCardData.cooldown <= 0)
            {
                currentRelic.relicCardData.cooldown = 0;
                currentRelic.useButton.SetActive(true); //enables button
                ultimateText.color = Color.green; //changes font colour
                ultimateText.text = "Ultimate Ready"; //updates card UI text
            }
        }
        RelicUpdate();
        APReset();
    }

    public void RelicAttacked()
    {
        currentRelic.useButton.SetActive(false); //disables button
        relicUsed = true;
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
        currentRelic.cooldownText.text = currentRelic.relicCardData.cooldown.ToString(); //updates prefab text from scriptable object
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

            currentRelic.useButton.SetActive(false); //disables button

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

    public void DealArmour() //Deals hero cards at start of game
    {
        if (equippedArmour.Count > 0)
        {
            armourTopDeck = equippedArmour[0];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCard tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedArmour.Remove(armourTopDeck); //removes the card from the deck
        equippedArmourObj.Add(tempCard.gameObject); //adds card to live list
        currentArmour = equippedArmourObj[0].GetComponent<ArmourCard>();
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
        defHero.heroCardData.armour = currentArmour.armourCardData.hp;
        heroMaxHP = defHero.heroCardData.hp;
    }

    public void DealRelic() //Deals hero cards at start of game
    {
        if (equippedRelic.Count > 0)
        {
            relicTopDeck = equippedRelic[0];
        }
        else
        {
            relicTopDeck = null;
        }
        UltimateData card = Instantiate(relicTopDeck); //instantiates instance of scriptable object
        UltimateCard tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData= card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedRelic.Remove(relicTopDeck); //removes card from list
        equippedRelicObj.Add(tempCard.gameObject); //adds card to live list
        currentRelic = equippedRelicObj[0].GetComponent<UltimateCard>();
        RelicMaxCooldown = currentRelic.relicCardData.cooldown; //assigns cooldown for reseting relic after use
    }

    public void DealWeaponHand() //Deals multiple weapon cards
    {
        int weaponsDealt = 3; //number of weapons to deal
        for (int i = 0; i < weaponsDealt; i++) //loops number of times equal to weaponsDealt variable
        {
            if (equippedWeapons.Count > 0)
            {
                DealWeapon(weaponTransform, equippedWeapons, equippedWeaponObj);
            }
        }
    }

    //public void DealWeapon(Transform spawnTransform, List<BaseCardData> dataList, List<GameObject> objectList) //Deals one weapon card
    //{
    //    if (dataList.Count > 0)
    //    {
    //        TopDeck = dataList[0];
    //    }
    //    else
    //    {
    //        TopDeck = null;
    //    }
    //    BaseCardData card = Instantiate(TopDeck); //instantiates instance of scriptable object
    //    WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
    //    tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
    //    tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
    //    dataList.Remove(weaponTopDeck); //removes card from list
    //    objectList.Add(tempCard.gameObject); //adds card to list
    //}

    public void DealWeapon(Transform spawnTransform, List<WeaponData> dataList, List<GameObject> objectList) //Deals one weapon card
    {
        if (dataList.Count > 0)
        {
            weaponTopDeck = dataList[0];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCard tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.weaponData = card; //assigns the instance of the scriptable object to the instance of the prefab
        dataList.Remove(weaponTopDeck); //removes card from list
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

    public void DropLoot()
    {
        if (weaponLoot1.Count > 0)
        {
            lootDropObj.SetActive(true); //enables menu
            menuToggle.isOn = !menuToggle.isOn; //toggles menu on
            lootCounter = lootDrop; //resets lootdrop counter
            DealLoot(lootChestTransform, weaponLoot1, lootChest); //deals card to lootdrop zone
        }
    }

    public void DealLoot(Transform spawnTransform, List<WeaponData> dataList, List<GameObject> objectList) //Deals one weapon card
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
        dataList.Remove(weaponTopDeck); //removes card from list
        objectList.Add(tempCard.gameObject); //adds card to list
        tempCard.equipButton.SetActive(true); //enables button
    }

    public void EquipWeapon(GameObject playedCard, WeaponData weaponData)
    {
        if (playedCard.transform.parent == lootChestTransform)
        {
            if (backPack.Count == 2)
            {
                print("Backpack full");
            }
            else
            {
                playedCard.transform.SetParent(backpackTransform.transform, false); //moves the card to the zone
                textEquipmentDeck.Add(weaponData.cardName);
                backPack.Add(playedCard); //adds to list
                lootChest.Remove(playedCard); //removes from list
            }
        }
        else
        {
            playedCard.transform.SetParent(lootChestTransform.transform, false); //moves the card to the zone
            backPack.Remove(playedCard); //removes from list
            textEquipmentDeck.Remove(weaponData.cardName);
            lootChest.Add(playedCard); //adds to list
        }
    }

    public void ClearLoot() //this function is called by the "Close" button on the loot drop menu
    {
        foreach(GameObject item in lootChest)
        {
            Destroy(item);
        }
        lootChest.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            T3LootTable();
        }
    }

    public void T3LootTable()
    {
        int t1Start = 1, t1end = 50 , t2Start = 51, t2end = 80, t3Start = 81 , t3end = 100; //values for loot drop tables
        int weaponStart = 1, relicStart = 2, armourStart = 3; //values for loot drop tables
        int tierRNG = UnityEngine.Random.Range(1, 101); //selects a random number between 1 and 100
        int typeRNG = UnityEngine.Random.Range(1, 4); //selects a random number between 1 and 100

        if (tierRNG >= t1Start && tierRNG <= t1end) //checks if loot drop number is within tier 1 value range
        {
            lootTier = lootNum.Tier1;
        }
        else if (tierRNG >= t2Start && tierRNG <= t2end) //checks if loot drop number is within tier 2 value range
        {
            lootTier = lootNum.Tier2;
        }
        else if (tierRNG >= t3Start && tierRNG <= t3end) //checks if loot drop number is within tier 3 value range
        {
            lootTier = lootNum.Tier3;
        }
        else
        {
            print("Invalid loot tier");
        }

        if (typeRNG == weaponStart) //checks if loot drop number is within tier 1 value range
        {
            lootType = lootTy.Weapon;
        }
        else if (typeRNG == relicStart) //checks if loot drop number is within tier 2 value range
        {
            lootType = lootTy.Ultimate;
        }
        else if (typeRNG == armourStart) //checks if loot drop number is within tier 3 value range
        {
            lootType = lootTy.Armour;
        }
        else
        {
            print("Invalid loot type");
        }
        print(lootTier + " " + lootType);
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

    public void ActivateRelic(UltimateData relicCardData)
    {
        if(relicCardData.heal > 0)
        {
            HealRelic(relicCardData);
        }
        if (relicCardData.dmg > 0)
        {
            tempRelic = relicCardData;
            WeaponTarget(relicCardData);
        }
        if (relicCardData.apBonus > 0)
        {
            APRelic(relicCardData);
        }
        RelicAttacked();
    }

    void HealRelic(UltimateData relicCardData)
    {
        if(defHero.heroCardData.hp + relicCardData.heal > heroMaxHP) //checks if healing will take hero past max HP
        {
            defHero.heroCardData.hp = heroMaxHP; //heals to max HP
        }
        else
        {
            defHero.heroCardData.hp += relicCardData.heal; //heals for heal amount on relic
        }
        HeroHPUpdate(); //updates card UI text
    }

    void APRelic(UltimateData relicCardData)
    {
        AP += relicCardData.apBonus; //updates action points
        APUpdate(); //updates UI text
    }

    public void WeaponTarget(WeaponData weaponData) //function for selecting weapon target
    {
        //print("standard targeting");
        if (AP - weaponData.ap < 0)
        {
            print("Not enough AP");
        }
        else
        {
            for (int i = 0; i < equippedCreaturesObj.Count; i++) //loop repeats for each creature on the board
            {
                CreatureCard attacker = equippedCreaturesObj[i].GetComponent<CreatureCard>();
                attacker.buttonObject.SetActive(false); //disables buttons on all creature card, so if the player changes target, it will disable out of range options.
            }

            tempWeapon = weaponData; //assigns the weapon clicked as the tempweapon
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

    public void WeaponTarget(UltimateData relicCardData) //function for selecting weapon target
    {
        //print("relic targeting");
        for (int i = 0; i < equippedCreaturesObj.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCard attacker = equippedCreaturesObj[i].GetComponent<CreatureCard>();
            attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
        }
    }

    public void WeaponAttack(GameObject creature, CreatureData creatureCard)
    {
        if(tempRelic == null)
        {
            AP -= tempWeapon.ap; //updates AP remaining
            APUpdate(); //updates UI text
            creatureCard.hp -= tempWeapon.dmg; //weapon deals dmg to creature
            //print("temp relic = null");
        }
        else
        {
            creatureCard.hp -= tempRelic.dmg; //relic deals dmg to creature
            //print("temp relic = not null");
            tempRelic = null;
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
        float animationDelay = 2.5f;
        GameObject.Find("MillerDealer").GetComponent<Animator>().SetBool("DealSequence", true); //Starts dealer animation
        yield return new WaitForSeconds(animationDelay);
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
            if (attackCreature.creatureCardData.dmg > currentArmour.armourCardData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attackCreature.creatureCardData.dmg - currentArmour.armourCardData.hp; //creature deals damage
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
    }

    public void NextLevel()
    {
        currentAiDeck = aiDeck2;
        SceneManager.LoadScene("GameScene");
        levelController.levelIncrement();
    }

    public void SaveGame()
    {
        FileStream file = new FileStream("save.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, textEquipmentDeck);
        file.Close();
        print("Saved");
    }

    public void LoadGame()
    {
        using (Stream stream = File.Open("save.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            List<string> tempList = (List<string>)bformatter.Deserialize(stream);

            for (int i = 0; i < tempList.Count; i++)
            {
                textEquipmentDeck.Add(tempList[i]);
            }
        }
        print("Loaded");
    }
}