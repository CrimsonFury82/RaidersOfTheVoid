﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    bool relicUsed;

    public Text apText, monstersText;

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    int AP, RelicMaxCooldown, heroMaxHP;

    GameObject defenderHeroObject;
        
    public GameObject victoryText, gameOverText;

    public Button endTurnButton;

    //Board zones for each group of cards
    public Transform enemyTransform, heroTransform, relicTransform, weaponTransform, armourTransform;

    //Deck lists
    public List<WeaponCardData> weaponDeck; 
    public List<RelicCardData> relicDeck;
    public List<ArmourCardData> armourDeck;
    public List<CreatureCardData> aiDeck;
    public List<HeroCardData> heroDeck;

    //Lists for the card prefabs on the board
    public List<GameObject> liveWeapons, liveRelic, liveArmour, liveCreatures, liveHero;

    //card prefabs
    public CreatureCardUI creatureCardTemplate;
    public HeroCardUI heroCardTemplate;
    public ArmourCardUI armourCardTemplate;
    public RelicCardUI relicCardTemplate;
    public WeaponCardUI weaponCardTemplate;

    HeroCardUI defHero;

    ArmourCardUI currentArmour;

    RelicCardUI currentRelic;

    public AnimationController animationController;

    public WeaponCardData tempWeapon;

    float endTurnDelay = 1.75f;

    //Temp objects for top card of each deck
    CreatureCardData creatureTopDeck;
    HeroCardData heroTopDeck;
    ArmourCardData armourTopDeck;
    WeaponCardData weaponTopDeck;
    RelicCardData relicTopDeck;
       
    void Shuffle(List<CreatureCardData> deck)
    {
        for (int i = 0; i < 1000; i++) //shuffles by swapping two random cards and repeating process 1000 times
        {   //comment text shows an example where rng1 result = 50 and rng2 result = 10
            int rng1 = UnityEngine.Random.Range(0, deck.Count);
            int rng2 = UnityEngine.Random.Range(0, deck.Count);
            CreatureCardData tempcard = deck[rng1]; //tempcard = card 50
            deck[rng1] = deck[rng2]; //card 50 = card 10
            deck[rng2] = tempcard; //card 10 = card 50
        }
    }

    public void Turns() //function for turn states
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

    public void Phases() //function for main\combat phase states
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

    void Start()
    {
        BeginGame();
    }

    void BeginGame() //function for starting the game.
    {
        relicUsed = false;
        Shuffle(aiDeck);
        DealArmour();
        DealHero();        
        DealRelic();
        DealWeaponHand();
        DealCreatureHand();
        turnState = turn.Player1;
        APReset();
    }

    public void EndTurn() //function for end of turn
    {
        if (liveCreatures.Count == 0 && aiDeck.Count == 0)
        {
            Victory();
            return;
        }

        turnPhase = phase.MainPhase;

        if (turnState == turn.Player1) //checks if currently Player 1's turn
        {
            turnState = turn.Player2;
        }
        else if (turnState == turn.Player2) //checks if currently Player 2's turn
        {
            PlayerUpkeep();
        }
        print("Turn = " + turnState);
        TurnStart();
    }

    void PlayerUpkeep()
    {
        turnState = turn.Player1;

        if(relicUsed == true)
        {
            currentRelic.relicCardData.cooldown = RelicMaxCooldown;
            relicUsed = false;
        }
        else
        {
            currentRelic.relicCardData.cooldown--;
            if (currentRelic.relicCardData.cooldown <= 0)
            {
                currentRelic.relicCardData.cooldown = 0;
                currentRelic.buttonObject.SetActive(true);
            }
        }
        RelicUpdate();
        APReset();
    }

    void APReset()
    {
        AP = 10;
        APUpdate();
    }

    void APUpdate()
    {
        apText.text = "Action Points = " + AP.ToString();
    }

    void HeroHPUpdate()
    {
        defHero.hpText.text = defHero.heroCardData.hp.ToString(); //updates UI HP text
    }

    void RelicUpdate()
    {
        currentRelic.cooldownText.text = currentRelic.relicCardData.cooldown.ToString(); //updates prefab with values from scriptable object
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is AI's turn
        {
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardUI weapon = liveWeapons[i].GetComponent<WeaponCardUI>();
                weapon.buttonObject.SetActive(false); //disables buttons on all weapon cards
            }

            if (liveCreatures.Count == 0) //checks if AI has creatures on the board
            {
                DealCreatureHand();
                EndTurn();
            }
            else
            {
                StartCoroutine(HeroAttackPhase()); //starts attack phase
            }
        }

        if (turnState == turn.Player1)
        {
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardUI weapon = liveWeapons[i].GetComponent<WeaponCardUI>();
                weapon.buttonObject.SetActive(true); //enables buttons on all creature cards
            }
        }
    }

    public void DealArmour() //Deals hero cards at start of game
    {
        if (armourDeck.Count > 0)
        {
            armourTopDeck = armourDeck[0];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourCardData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCardUI tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourDeck.Remove(armourTopDeck);
        liveArmour.Add(tempCard.gameObject); //adds card to hero list
        currentArmour = liveArmour[0].GetComponent<ArmourCardUI>();
    }

    public void DealHero() //Deals hero cards at start of game
    {
        if (heroDeck.Count > 0)
        {
            heroTopDeck = heroDeck[0];
        }
        else
        {
            heroTopDeck = null;
        }
        HeroCardData card = Instantiate(heroTopDeck); //instantiates instance of scriptable object
        HeroCardUI tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card onto board
        tempCard.heroCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        heroDeck.Remove(heroTopDeck);
        liveHero.Add(tempCard.gameObject); //adds card to hero list
        defHero = liveHero[0].GetComponent<HeroCardUI>();
        defHero.heroCardData.armour = currentArmour.armourCardData.hp;
        heroMaxHP = defHero.heroCardData.hp;
    }

    public void DealRelic() //Deals hero cards at start of game
    {
        if (relicDeck.Count > 0)
        {
            relicTopDeck = relicDeck[0];
        }
        else
        {
            relicTopDeck = null;
        }
        RelicCardData card = Instantiate(relicTopDeck); //instantiates instance of scriptable object
        RelicCardUI tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData= card; //assigns the instance of the scriptable object to the instance of the prefab
        relicDeck.Remove(relicTopDeck); //removes card from list
        liveRelic.Add(tempCard.gameObject); //adds card to list
        currentRelic = liveRelic[0].GetComponent<RelicCardUI>();
        RelicMaxCooldown = currentRelic.relicCardData.cooldown;
    }

    public void DealWeaponHand() //Deals multiple weapon cards
    {
        int weaponsDealt = 3; //number of weapons to deal
        for (int i = 0; i < weaponsDealt; i++) //loops number of times equal to weaponsDealt variable
        {
            if (weaponDeck.Count > 0)
            {
                DealWeapon();
            }
        }
    }

    public void DealWeapon() //Deals one weapon card
    {
        if (weaponDeck.Count > 0)
        {
            weaponTopDeck = weaponDeck[0];
        }
        else
        {
            weaponTopDeck = null;
        }
        WeaponCardData card = Instantiate(weaponTopDeck); //instantiates instance of scriptable object
        WeaponCardUI tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponTransform.transform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponDeck.Remove(weaponTopDeck);
        liveWeapons.Add(tempCard.gameObject); //adds card to hero list
    }

    public void DealCreatureHand() //Deals multiple creature cards
    {
        int monstersDealt = 4; //number of monsters to deal
        for (int i = 0; i < monstersDealt; i++) //loops number of times equal to monstersDealt variable
        {
            if (aiDeck.Count > 0)
            {
                DealCreature();
            }
        }
        monstersText.text = "Monster Deck = " + aiDeck.Count.ToString(); //updates UI text
    }

    public void DealCreature() //Deals one creature card
    {
        if (aiDeck.Count > 0)
        {
            creatureTopDeck = aiDeck[0];
        }
        else
        {
            creatureTopDeck = null;
        }

        CreatureCardData card = Instantiate(creatureTopDeck); //instantiates an instance of the carddata scriptable object
        CreatureCardUI dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(enemyTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        aiDeck.Remove(creatureTopDeck); //removes card from deck list
        liveCreatures.Add(dealtCard.gameObject);
    }

    public void WeaponTarget(WeaponCardData weaponCardData, Button button) //function for selecting weapon target
    {
        if (AP - weaponCardData.ap < 0)
        {
            print("Not enough AP");
        }
        else
        {
            tempWeapon = weaponCardData; //assigns the weapon clicked as the tempweapon
            print("Select target");
            int range = weaponCardData.range;

            if (liveCreatures.Count < range)
            {
                range = liveCreatures.Count;
            }

            for (int i = 0; i < range; i++) //loop repeats for each creature on the board
            {
                CreatureCardUI attacker = liveCreatures[i].GetComponent<CreatureCardUI>();
                attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
            }
        }
    }

    public void SupportRelic(RelicCardData relicCardData)
    {
        if (defHero.heroCardData.hp + relicCardData.heal > heroMaxHP)
        {
            defHero.heroCardData.hp = heroMaxHP;
        }
        else
        {
            defHero.heroCardData.hp += relicCardData.heal;
        }
        HeroHPUpdate();
        RelicAttacked();
    }

    public void RelicAttacked()
    {
        currentRelic.buttonObject.SetActive(false);
        relicUsed = true;
        currentRelic.cooldownText.text = "Used this turn"; //updates prefab with values from scriptable object
    }

    public void WeaponAttack(GameObject creature, CreatureCardData creatureCard, Button button)
    {
        //print(creatureCard.name + " HP = " + creatureCard.hp);
        //print(tempWeapon.name + " DMG = " + tempWeapon.dmg + " AP = " + tempWeapon.ap);
        AP -= tempWeapon.ap; //updates AP remaining
        creatureCard.hp -= tempWeapon.dmg; //deals dmg to creature
        //print(creatureCard.name + " HP = " + creatureCard.hp);
        APUpdate();
        CreatureCardUI defCreature = creature.GetComponent<CreatureCardUI>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCardUI deadCreature = creature.GetComponent<CreatureCardUI>();
            Destroy(deadCreature.gameObject); //destroys the creature
            liveCreatures.Remove(deadCreature.gameObject); //removes destroyed creature from attackers list
        }

        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCardUI attacker = liveCreatures[i].GetComponent<CreatureCardUI>();
            attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
        }
    }

    public IEnumerator HeroAttackPhase()
    {
        turnPhase = phase.CombatPhase;
        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature fighting the hero
        {
            defenderHeroObject = liveHero[0];
            CreatureCardUI attackCreature = liveCreatures[i].GetComponent<CreatureCardUI>();
            StartCoroutine(animationController.AttackMove(liveCreatures[i], defenderHeroObject));
            attackCreature.PlaySound();
            if (attackCreature.creatureCardData.dmg > currentArmour.armourCardData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attackCreature.creatureCardData.dmg - currentArmour.armourCardData.hp; //creature deals damage
            }
            HeroHPUpdate();
            print("Hero hp = " + defHero.heroCardData.hp);
            float combatDelay = 1.4f;
            yield return new WaitForSeconds(combatDelay);
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
        gameOverText.SetActive(true);
    }

    public void Victory() //function for winning the game
    {
        victoryText.SetActive(true);
    }
}