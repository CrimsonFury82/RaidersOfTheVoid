using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states
    public enum lootNum {Tier1, Tier2, Tier3}; //List of states
    public enum lootTy {Weapon, Ultimate, Armour } //List of states

    bool relicUsed;

    public Text apText, monstersText, turnText, gameoverText; //UI text

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    public lootNum lootTier; //State for Loot drop tier

    public lootTy lootType; //State for Loot drop type

    int AP, RelicMaxCooldown, heroMaxHP, lootdropCounter = 3;

    public GameObject gameoverBackground, lootDrop, backPack, menuButton, exitButton, continueButton; //menu objects that will be toggled off and on

    Button endTurnButton;

    //Board zones for each group of cards
    public Transform enemyTransform, heroTransform, relicTransform, weaponTransform, armourTransform, lootTransform, backpackTransform;

    //Deck lists
    public List<WeaponCardData> weaponLoot1, weaponLoot2, weaponLoot3;
    public List<RelicCardData> relicLoot1, relicLoot2, relicLoot3;
    public List<ArmourCardData> armourLoot1, armourLoot2, armourLoot3;
    public List<WeaponCardData> equippedWeapons; 
    public List<RelicCardData> equippedRelic;
    public List<ArmourCardData> equippedArmour;
    public List<HeroCardData> equippedHero;
    public List<CreatureCardData> aiDeck1, aiDeck2, aiDeck3;
    public List<BaseCardData> lootdeckTest;

    //Lists of card prefabs on the board
    public List<GameObject> liveWeapons, liveRelic, liveArmour, liveCreatures, liveHero, liveLoot;

    //card prefabs
    public CreatureCardPrefab creatureCardTemplate;
    public HeroCardPrefab heroCardTemplate;
    public ArmourCardPrefab armourCardTemplate;
    public RelicCardPrefab relicCardTemplate;
    public WeaponCardPrefab weaponCardTemplate;

    //prefabs instances on the board
    HeroCardPrefab defHero;
    ArmourCardPrefab currentArmour;
    RelicCardPrefab currentRelic;

    public AnimationController animationController;

    WeaponCardData tempWeapon; //currently selected weapon
    RelicCardData tempRelic = null; //current selected attack relic

    float endTurnDelay = 1.75f;

    //Temp objects for top card of each deck
    CreatureCardData creatureTopDeck;
    HeroCardData heroTopDeck;
    ArmourCardData armourTopDeck;
    WeaponCardData weaponTopDeck;
    RelicCardData relicTopDeck;
       
    void CreatureShuffle(List<CreatureCardData> deck)
    {
        for (int i = 0; i < 1000; i++) //shuffles by swapping two random cards and repeating process 1000 times
        {   
            int rng1 = UnityEngine.Random.Range(0, deck.Count); //first randomly selected card number
            int rng2 = UnityEngine.Random.Range(0, deck.Count); //second randomly selected card number
            CreatureCardData tempcard = deck[rng1]; //tempcard to store copy of card 1
            deck[rng1] = deck[rng2]; //swaps card 2 into card 1's position in list
            deck[rng2] = tempcard; //swaps temp copy of card 1 into card 2's position in list
        }
    }

    public void Turns() //case switches for turn states
    {   switch (turnState)
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

    void Start()
    {
        BeginGame();
    }

    void BeginGame() //function for starting the game.
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MenuMusicController>().StopMusic();
        relicUsed = false;
        CreatureShuffle(aiDeck1);
        DealArmour();
        DealHero();        
        DealRelic();
        DealWeaponHand();
        DealCreatureHand();
        lootdropCounter++;
        turnState = turn.Player1;
        if (turnState == turn.Player1) //checks if it is the Players's turn
        {
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardPrefab weapon = liveWeapons[i].GetComponent<WeaponCardPrefab>();
                weapon.useButton.SetActive(true); //enables buttons on all weapon cards during player turn
            }
            if (currentRelic.relicCardData.cooldown == 0) //Checks if Ultimte is ready
            {
                currentRelic.useButton.SetActive(true); //enables button
            }
        }
        APReset();
    }

    public void EndTurn() //function for end of turn
    {
        if (liveCreatures.Count == 0 && aiDeck1.Count == 0) //checks if all enemies have been destroy for victory condition.
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
            currentRelic.ultimateText.text = "Ultimate\ncharging"; //updates card UI text
            RelicUpdate();
        }
        else
        {
            currentRelic.relicCardData.cooldown--; //updates cooldown
            if (currentRelic.relicCardData.cooldown <= 0)
            {
                currentRelic.relicCardData.cooldown = 0;
                currentRelic.useButton.SetActive(true); //enables button
                currentRelic.ultimateText.color = Color.green; //changes font colour
                currentRelic.ultimateText.text = "Ultimate\nReady"; //updates card UI text
            }
        }
        RelicUpdate();
        APReset();
    }

    public void RelicAttacked()
    {
        currentRelic.useButton.SetActive(false); //disables button
        relicUsed = true;
        //tempRelic = null;
        currentRelic.ultimateText.color = Color.red; //changes font colour
        currentRelic.ultimateText.text = "Ultimate\nUsed"; //updates card UI text
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
        monstersText.text = "Monsters remaining: " + (aiDeck1.Count + liveCreatures.Count).ToString(); 
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is the AI's turn
        {
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardPrefab weapon = liveWeapons[i].GetComponent<WeaponCardPrefab>();
                weapon.useButton.SetActive(false); //disables buttons on all weapon cards during AI turn
            }

            currentRelic.useButton.SetActive(false); //disables button

            if (liveCreatures.Count == 0) //checks if AI has creatures on the board
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
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardPrefab weapon = liveWeapons[i].GetComponent<WeaponCardPrefab>();
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
        ArmourCardData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCardPrefab tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedArmour.Remove(armourTopDeck); //removes the card from the deck
        liveArmour.Add(tempCard.gameObject); //adds card to live list
        currentArmour = liveArmour[0].GetComponent<ArmourCardPrefab>();
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
        HeroCardData card = Instantiate(heroTopDeck); //instantiates instance of scriptable object
        HeroCardPrefab tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card onto board
        tempCard.heroCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedHero.Remove(heroTopDeck);
        liveHero.Add(tempCard.gameObject); //adds card to live list
        defHero = liveHero[0].GetComponent<HeroCardPrefab>();
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
        RelicCardData card = Instantiate(relicTopDeck); //instantiates instance of scriptable object
        RelicCardPrefab tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData= card; //assigns the instance of the scriptable object to the instance of the prefab
        equippedRelic.Remove(relicTopDeck); //removes card from list
        liveRelic.Add(tempCard.gameObject); //adds card to live list
        currentRelic = liveRelic[0].GetComponent<RelicCardPrefab>();
        RelicMaxCooldown = currentRelic.relicCardData.cooldown; //assigns cooldown for reseting relic after use
    }

    public void DealWeaponHand() //Deals multiple weapon cards
    {
        int weaponsDealt = 3; //number of weapons to deal
        for (int i = 0; i < weaponsDealt; i++) //loops number of times equal to weaponsDealt variable
        {
            if (equippedWeapons.Count > 0)
            {
                DealWeapon(weaponTransform, equippedWeapons, liveWeapons);
            }
        }
    }

    //public void DealWeapon(Transform spawnTransform, List<BaseCardData> dataList, List<GameObject> objectList) //Deals one weapon card
    //{
    //    //if (dataList.Count > 0)
    //    //{
    //    //    weaponTopDeck = dataList[0];
    //    //}
    //    //else
    //    //{
    //    //    weaponTopDeck = null;
    //    //}
    //    WeaponCardData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
    //    WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
    //    tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
    //    tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
    //    dataList.Remove(weaponTopDeck); //removes card from list
    //    objectList.Add(tempCard.gameObject); //adds card to list
    //}

    public void DealWeapon(Transform spawnTransform, List<WeaponCardData> dataList, List<GameObject> objectList) //Deals one weapon card
    {
        if (dataList.Count > 0)
        {
            weaponTopDeck = dataList[0];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponCardData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(spawnTransform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        dataList.Remove(weaponTopDeck); //removes card from list
        objectList.Add(tempCard.gameObject); //adds card to list
    }

    public void DealCreatureHand() //Deals multiple creature cards
    {
        StartCoroutine(DealerAnimation()); //starts animation
        lootdropCounter--;
        if(lootdropCounter == 0)
        {
            DropLoot();   
        }

        int dealMonsters = 4; //number of monsters to deal
        for (int i = 0; i < dealMonsters; i++) //loops number of times equal to dealMonsters variable
        {
            if (aiDeck1.Count > 0)
            {
                DealCreature();
            }
        }
        MonstersUpdate();
    }

    //public void DropLoot()
    //{
    //    if(weaponLoot1.Count > 0)
    //    {
    //        lootDrop.SetActive(true); //enables menu
    //        backPack.SetActive(true); //enables menu
    //        menuButton.SetActive(false); //disables menu
    //        lootdropCounter = 3; //resets lootdrop counter
    //        DealWeapon(lootTransform, weaponLoot1, liveLoot);
    //    }
    //}

    public void DropLoot()
    {
        if (weaponLoot1.Count > 0)
        {
            lootDrop.SetActive(true); //enables menu
            backPack.SetActive(true); //enables menu
            menuButton.SetActive(false); //disables menu
            lootdropCounter = 3; //resets lootdrop counter
            DealWeapon(lootTransform, lootdeckTest, liveLoot);
        }
    }

    public void ClearLoot() //this function is called by the "Close" button on the loot drop menu
    {
        Destroy(liveLoot[0].gameObject);
        liveLoot.Clear();
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
        if (aiDeck1.Count > 0)
        {
            creatureTopDeck = aiDeck1[0];
        }
        else
        {
            creatureTopDeck = null;
        }

        CreatureCardData card = Instantiate(creatureTopDeck); //instantiates an instance of the carddata scriptable object
        CreatureCardPrefab dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(enemyTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        aiDeck1.Remove(creatureTopDeck); //removes card from deck list
        liveCreatures.Add(dealtCard.gameObject);
    }

    public void ActivateRelic(RelicCardData relicCardData)
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

    void HealRelic(RelicCardData relicCardData)
    {
        if(defHero.heroCardData.hp + relicCardData.heal > heroMaxHP)
        {
            defHero.heroCardData.hp = heroMaxHP;
        }
        else
        {
            defHero.heroCardData.hp += relicCardData.heal;
        }
        HeroHPUpdate();
    }

    void APRelic(RelicCardData relicCardData)
    {
        AP += relicCardData.apBonus;
        APUpdate();
    }

    public void WeaponTarget(WeaponCardData weaponCardData) //function for selecting weapon target
    {
        //print("standard targeting");
        if (AP - weaponCardData.ap < 0)
        {
            print("Not enough AP");
        }
        else
        {
            for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
            {
                CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
                attacker.buttonObject.SetActive(false); //disables buttons on all creature card, so if the player changes target, it will disable out of range options.
            }

            tempWeapon = weaponCardData; //assigns the weapon clicked as the tempweapon
            int tempRange = weaponCardData.range;

            if (liveCreatures.Count < weaponCardData.range)
            {
                tempRange = liveCreatures.Count;
            }

            for (int i = 0; i < tempRange; i++) //loop repeats for each creature on the board
            {
                //print("temp range " + tempRange);
                CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
                attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
            }
        }
    }

    public void WeaponTarget(RelicCardData relicCardData) //function for selecting weapon target
    {
        //print("relic targeting");
        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
            attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
        }
    }

    public void WeaponAttack(GameObject creature, CreatureCardData creatureCard)
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
        CreatureCardPrefab defCreature = creature.GetComponent<CreatureCardPrefab>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCardPrefab deadCreature = creature.GetComponent<CreatureCardPrefab>();
            Destroy(deadCreature.gameObject); //destroys the creature
            liveCreatures.Remove(deadCreature.gameObject); //removes destroyed creature from attackers list
            MonstersUpdate(); //updates UI text
        }

        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
            attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
        }
    }

    public IEnumerator DealerAnimation()
    {
        float animationDelay = 2.5f;
        GameObject.Find("MillerDealer").GetComponent<Animator>().SetBool("DealSequence", true); //Kyle
        yield return new WaitForSeconds(animationDelay);
        GameObject.Find("MillerDealer").GetComponent<Animator>().SetBool("DealSequence", false); //Kyle
    }

    public IEnumerator HeroAttackPhase()
    {
        turnPhase = phase.CombatPhase; //sets state of combat phase
        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardPrefab attackCreature = liveCreatures[i].GetComponent<CreatureCardPrefab>();
            StartCoroutine(animationController.AttackMove(liveCreatures[i], liveHero[0])); //starts coroutine for moving attacking card
            attackCreature.PlaySound(); //plays sound effect
            if (attackCreature.creatureCardData.dmg > currentArmour.armourCardData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attackCreature.creatureCardData.dmg - currentArmour.armourCardData.hp; //creature deals damage
            }
            HeroHPUpdate(); //updates UI text
            float combatDelay = 1.3f;

            if (i!=liveCreatures.Count-1)
            {
                yield return new WaitForSeconds(combatDelay);
            }
            else if (i == liveCreatures.Count - 1)
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
        exitButton.SetActive(true);
        continueButton.SetActive(true);
    }
}