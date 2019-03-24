using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    public Text apText, monstersText;

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    int turnsRemaining, AP;

    GameObject defenderHeroObject;
        
    public GameObject victoryText, gameOverText;

    public Button endTurnButton;

    public Transform enemyTransform, heroTransform, relicTransform, weaponTransform, armourTransform; //Board zones for each group of cards

    //Deck lists
    public List<WeaponCardData> weaponDeck; 
    public List<RelicCardData> relicDeck;
    public List<ArmourCardData> armourDeck;
    public List<CreatureCardData> aiDeck;
    public List<HeroCardData> heroDeck;

    //Lists for the card prefabs on the board
    public List<GameObject> liveWeapons, liveRelic, liveArmour, liveCreatures, liveHero; 

    public CreatureCardUI creatureCardTemplate; //card prefab
    public HeroCardUI heroCardTemplate;
    public ArmourCardUI armourCardTemplate;
    public RelicCardUI relicCardTemplate;
    public WeaponCardUI weaponCardTemplate;

    HeroCardUI defHero;

    ArmourCardUI currentArmour;

    public AnimationController animationController;

    public WeaponCardData tempWeapon;

    float endTurnDelay = 1.75f;

    //Temp object for top card of each deck
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

    public void Phases() //function for phase states
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

    void BeginGame() //function for start of game.
    {
        Shuffle(aiDeck);
        DealArmour();
        DealHero();        
        DealRelic();
        DealWeaponHand();
        DealCreatureHand();
        turnState = turn.Player1;
        APReset();
    }

    public void EndTurn() //function for Start of turn
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
            turnState = turn.Player1;
            APReset();
        }
        print("Turn = " + turnState);
        TurnStart();
    }

    void APReset()
    {
        AP = 10;
        apText.text = "Action Points = " + AP.ToString();
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is AI's turn
        {
            if(liveCreatures.Count == 0) //checks if AI has creatures on the board
            {
                DealCreatureHand();
                EndTurn();
            }
            else
            {
                StartCoroutine(HeroAttackPhase()); //starts attack phase
            }
        }

        if (turnState == turn.Player1 & turnsRemaining > 1)
        {
            turnsRemaining--;
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
        relicDeck.Remove(relicTopDeck);
        liveRelic.Add(tempCard.gameObject); //adds card to hero list
    }

    public void DealWeaponHand()
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

    public void DealWeapon() //Deals hero cards at start of game
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

    public void DealCreatureHand()
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

    public void DealCreature() //deals monster card to AI monster area
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

    public void WeaponTarget(WeaponCardData weaponCardData, Button button)
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

            for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature on the board
            {
                CreatureCardUI attacker = liveCreatures[i].GetComponent<CreatureCardUI>();
                attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
            }

            for (int i = 0; i < range; i++) //loop repeats for each creature on the board
            {
                CreatureCardUI attacker = liveCreatures[i].GetComponent<CreatureCardUI>();
                attacker.buttonObject.SetActive(true); //enables buttons on creature cards in range
            }
        }
    }

    public void WeaponAttack(GameObject creature, CreatureCardData creatureCard, Button button)
    {
        print(creatureCard.name + " HP = " + creatureCard.hp);
        print(tempWeapon.name + " DMG = " + tempWeapon.dmg + " AP = " + tempWeapon.ap);
        AP -= tempWeapon.ap; //updates AP remaining
        creatureCard.hp -= tempWeapon.dmg; //deals dmg to creature
        print(creatureCard.name + " HP = " + creatureCard.hp);
        apText.text = "Action Points = " + AP.ToString(); //updates UI text
        CreatureCardUI defCreature = creature.GetComponent<CreatureCardUI>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCardUI deadCreature = creature.GetComponent<CreatureCardUI>();
            GameObject deadGameObject = deadCreature.gameObject;
            Destroy(deadGameObject); //destroys the creature
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
        float combatDelay = 1f;
        yield return new WaitForSeconds(combatDelay);
        turnPhase = phase.CombatPhase;

        for (int i = 0; i < liveCreatures.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardUI attacker = liveCreatures[i].GetComponent<CreatureCardUI>();
            defenderHeroObject = liveHero[0];
            animationController.AttackStart(liveCreatures[i], defenderHeroObject);
            if(attacker.creatureCardData.dmg > currentArmour.armourCardData.hp) //checks if creature attack is strong enough to pierce armour
            {
                defHero.heroCardData.hp -= attacker.creatureCardData.dmg - currentArmour.armourCardData.hp; //creature deals damage
            }
            defHero.hpText.text = defHero.heroCardData.hp.ToString(); //updates UI HP text
            print("Hero hp = " + defHero.heroCardData.hp);
        }

        if (defHero.heroCardData.hp <= 0) //checks if hero is dead
        {
            GameOver();
            CancelInvoke();
        }
        Invoke("TurnUpkeep", endTurnDelay);
    }

    public void GameOver() //function for losing the game
    {
        gameOverText.SetActive(true);

        print("You Lose");
    }

    public void Victory() //function for winning the game
    {
        victoryText.SetActive(true);

        print("You Win");
    }
}