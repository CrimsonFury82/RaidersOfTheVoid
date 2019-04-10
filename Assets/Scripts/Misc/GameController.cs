using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    bool relicUsed;

    public Text apText, monstersText, turnText;

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
    //public List<LootCardData> lookDeck;

    //Lists for the card prefabs on the board
    public List<GameObject> liveWeapons, liveRelic, liveArmour, liveCreatures, liveHero, liveLoot;

    //card prefabs
    public CreatureCardPrefab creatureCardTemplate;
    public HeroCardPrefab heroCardTemplate;
    public ArmourCardPrefab armourCardTemplate;
    public RelicCardPrefab relicCardTemplate;
    public WeaponCardPrefab weaponCardTemplate;

    HeroCardPrefab defHero;

    ArmourCardPrefab currentArmour;

    RelicCardPrefab currentRelic;

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
        {   
            int rng1 = UnityEngine.Random.Range(0, deck.Count); //first randomly selected card number
            int rng2 = UnityEngine.Random.Range(0, deck.Count); //second randomly selected card number
            CreatureCardData tempcard = deck[rng1]; //tempcard to store copy of card 1
            deck[rng1] = deck[rng2]; //swaps card 2 into card 1's position in list
            deck[rng2] = tempcard; //swaps temp copy of card 1 into card 2's position in list
        }
    }

	void Shuffle(List<LootCardData> deck)
	{
		for (int i = 0; i < 1000; i++) //shuffles by swapping two random cards and repeating process 1000 times
		{   
			int rng1 = UnityEngine.Random.Range(0, deck.Count); //first randomly selected card number
			int rng2 = UnityEngine.Random.Range(0, deck.Count); //second randomly selected card number
			LootCardData tempcard = deck[rng1]; //tempcard to store copy of card 1
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

    public void Phases() //case switches for main\combat phase states
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
        if (turnState == turn.Player1) //checks if it is the Players's turn
        {
            for (int i = 0; i < liveWeapons.Count; i++) //loop repeats for each weapon on the board
            {
                WeaponCardPrefab weapon = liveWeapons[i].GetComponent<WeaponCardPrefab>();
                weapon.useButton.SetActive(true); //enables buttons on all weapon cards during player turn
            }
        }
        APReset();
    }

    public void EndTurn() //function for end of turn
    {
        if (liveCreatures.Count == 0 && aiDeck.Count == 0) //checks if all enemies have been destroy for victory condition.
        {
            Victory();
            return;
        }

        turnPhase = phase.MainPhase;

        if (turnState == turn.Player1) //checks if currently Player 1's turn
        {
            turnState = turn.Player2;
            turnText.text = "Enemy turn";
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
        turnText.text = "Your turn";

        if (relicUsed == true)
        {
            currentRelic.relicCardData.cooldown = RelicMaxCooldown;
            relicUsed = false;
            RelicUpdate();
        }
        else
        {
            currentRelic.relicCardData.cooldown--;
            RelicUpdate();
            if (currentRelic.relicCardData.cooldown <= 0)
            {
                currentRelic.relicCardData.cooldown = 0;
                currentRelic.useButton.SetActive(true); //enables button
                currentRelic.cooldownText.text = "Ready"; //updates card UI text
            }
        }
        APReset();
    }

    void APReset() //resets action points at start of turn
    {
        AP = 10;
        APUpdate();
    }

    void APUpdate() //updates UI text
    {
        apText.text = "Action Points = " + AP.ToString();
    }

    void HeroHPUpdate() //updates UI text
    {
        defHero.hpText.text = defHero.heroCardData.hp.ToString();
    }

    void RelicUpdate() //updates UI text
    {
        currentRelic.cooldownText.text = currentRelic.relicCardData.cooldown.ToString(); //updates prefab with values from scriptable object
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
        if (armourDeck.Count > 0)
        {
            armourTopDeck = armourDeck[0];
        }
        else
        {
            armourTopDeck = null;
        }
        ArmourCardData card = Instantiate(armourTopDeck); //instantiates instance of scriptable object
        ArmourCardPrefab tempCard = Instantiate(armourCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(armourTransform.transform, false); //moves card onto board
        tempCard.armourCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        armourDeck.Remove(armourTopDeck); //removes the card from the deck
        liveArmour.Add(tempCard.gameObject); //adds card to live list
        currentArmour = liveArmour[0].GetComponent<ArmourCardPrefab>();
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
        HeroCardPrefab tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card onto board
        tempCard.heroCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        heroDeck.Remove(heroTopDeck);
        liveHero.Add(tempCard.gameObject); //adds card to live list
        defHero = liveHero[0].GetComponent<HeroCardPrefab>();
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
        RelicCardPrefab tempCard = Instantiate(relicCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(relicTransform.transform, false); //moves card onto board
        tempCard.relicCardData= card; //assigns the instance of the scriptable object to the instance of the prefab
        relicDeck.Remove(relicTopDeck); //removes card from list
        liveRelic.Add(tempCard.gameObject); //adds card to live list
        currentRelic = liveRelic[0].GetComponent<RelicCardPrefab>();
        RelicMaxCooldown = currentRelic.relicCardData.cooldown; //assigns cooldown for reseting relic after use
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
        WeaponCardPrefab tempCard = Instantiate(weaponCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(weaponTransform.transform, false); //moves card onto board
        tempCard.weaponCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        weaponDeck.Remove(weaponTopDeck);
        liveWeapons.Add(tempCard.gameObject); //adds card to live list
    }

    public void DealCreatureHand() //Deals multiple creature cards
    {
        int dealMonsters = 4; //number of monsters to deal
        for (int i = 0; i < dealMonsters; i++) //loops number of times equal to dealMonsters variable
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
        CreatureCardPrefab dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(enemyTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        aiDeck.Remove(creatureTopDeck); //removes card from deck list
        liveCreatures.Add(dealtCard.gameObject);
    }

    public void WeaponTarget(WeaponCardData weaponCardData) //function for selecting weapon target
    {
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
            print("Select target");
            int range = weaponCardData.range;

            if (liveCreatures.Count < range)
            {
                range = liveCreatures.Count;
            }

            for (int i = 0; i < range; i++) //loop repeats for each creature on the board
            {
                CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
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
        currentRelic.useButton.SetActive(false); //disables button
        relicUsed = true;
        currentRelic.cooldownText.text = "Used"; //updates card UI text
    }

    public void WeaponAttack(GameObject creature, CreatureCardData creatureCard, Button button)
    {
        AP -= tempWeapon.ap; //updates AP remaining
        creatureCard.hp -= tempWeapon.dmg; //deals dmg to creature
        APUpdate();
        CreatureCardPrefab defCreature = creature.GetComponent<CreatureCardPrefab>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCardPrefab deadCreature = creature.GetComponent<CreatureCardPrefab>();
            Destroy(deadCreature.gameObject); //destroys the creature
            liveCreatures.Remove(deadCreature.gameObject); //removes destroyed creature from attackers list
        }

        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCardPrefab attacker = liveCreatures[i].GetComponent<CreatureCardPrefab>();
            attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
        }
    }

    public IEnumerator HeroAttackPhase()
    {
        turnPhase = phase.CombatPhase;
        defenderHeroObject = liveHero[0];
        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardPrefab attackCreature = liveCreatures[i].GetComponent<CreatureCardPrefab>();
            StartCoroutine(animationController.AttackMove(liveCreatures[i], defenderHeroObject));
            attackCreature.PlaySound();
            if (attackCreature.creatureCardData.dmg > currentArmour.armourCardData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attackCreature.creatureCardData.dmg - currentArmour.armourCardData.hp; //creature deals damage
            }
            HeroHPUpdate();
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
        gameOverText.SetActive(true);
    }

    public void Victory() //function for winning the game
    {
        victoryText.SetActive(true);
    }
}