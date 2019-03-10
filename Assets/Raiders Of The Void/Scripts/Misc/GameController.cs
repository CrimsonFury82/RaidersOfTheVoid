using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    public Text apText;

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    int turnsRemaining, AP;

    GameObject defenderHeroObject;

    public Button endTurnButton;

    public Transform enemyTransform, heroTransform; //Board areas for each players cards

    public List<CreatureCardData> aiDeck; //Deck list

    public List<HeroCardData> heroDeck; //Deck list

    public List<GameObject> attackers, liveHeroes; //lists for the card prefabs on the board and dead cards

    public CreatureCardUI creatureCardTemplate; //card prefab

    public HeroCardUI heroCardTemplate;

    HeroCardUI defHero;

    public AnimationController animationController;

    public WeaponCardData tempWeapon;

    float endTurnDelay = 1.75f;

    CreatureCardData topDeckCard;

    void Shuffle(List<CreatureCardData> deck)
    {
        for (int i = 0; i < 1000; i++) //shuffles by swapping two random cards and repeating process 1000 times
        {   //comment text shows an example where rng1 result = 50 and rng2 result = 10
            int rng1 = Random.Range(0, deck.Count);
            int rng2 = Random.Range(0, deck.Count);
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
        DealHero(heroDeck[0], heroTransform);
        defHero = liveHeroes[0].GetComponent<HeroCardUI>();
        DealHand();
        turnState = turn.Player1;
        APUpdate();
    }

    public void TurnUpkeep() //function for Start of turn
    {
        if (attackers.Count == 0 && aiDeck.Count == 0)
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
            APUpdate();
        }
        print("Turn = " + turnState);
        TurnStart();
    }

    void APUpdate()
    {
        AP = 10;
        apText.text = "AP = " + AP.ToString();
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is AI's turn
        {
            if(attackers.Count == 0) //checks if AI has creatures on the board
            {
                DealHand();
                TurnUpkeep();
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

    public void EndTurn()
    {
        TurnUpkeep();
    }

    public void DealHand()
    {
        for(int i = 0; i < 4; i++)
        {
            if (aiDeck.Count > 0)
            {
                DealCard();
            }
        }
    }

    public void WeaponTarget(WeaponCardData weaponCardData, Button button)
    {
        if(AP - weaponCardData.ap <0)
        {
            print("Not enough AP");
        }
        else {
            tempWeapon = weaponCardData; //assigns the weapon clicked as the tempweapon
            print("Select target");
            for (int i = 0; i < attackers.Count; i++) //loop repeats for each creature on the board
            {
                CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
                attacker.buttonObject.SetActive(true); //enables buttons on all the creature cards
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
        apText.text = "AP = " + AP.ToString(); //updates UI text
        CreatureCardUI defCreature = creature.GetComponent<CreatureCardUI>(); //creatures a reference to the creature
        defCreature.hpText.text = defCreature.creatureCardData.hp.ToString(); //updates UI text

        if (creatureCard.hp <= 0) //checks if creature is dead
        {
            CreatureCardUI deadCreature = creature.GetComponent<CreatureCardUI>();
            GameObject deadGameObject = deadCreature.gameObject;
            GameObject.Destroy(deadGameObject); //destroys the creature
            attackers.Remove(deadCreature.gameObject); //removes destroyed creature from attackers list
        }

        for (int i = 0; i < attackers.Count; i++) //loop repeats for each creature on the board
        {
            CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
            attacker.buttonObject.SetActive(false); //disables buttons on all creature cards
        }
    }

    public void DealHero(HeroCardData hero, Transform heroTransform) //Deals hero cards at start of game
    {
        HeroCardData card = Instantiate(hero); //instantiates instance of scriptable object
        HeroCardUI tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card onto board
        tempCard.heroCardData = card; //assigns the instance of the scriptable object to the instance of the prefab
        liveHeroes.Add(tempCard.gameObject); //adds card to hero list
    }

    public void DealCard() //deals monster card to AI monster area
    {
        if (aiDeck.Count > 0)
        {
            topDeckCard = aiDeck[0];
        }
        else
        {
            topDeckCard = null;
        }

        CreatureCardData card = Instantiate(topDeckCard); //instantiates an instance of the carddata scriptable object
        CreatureCardUI dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(enemyTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        aiDeck.Remove(topDeckCard); //removes card from deck list
        attackers.Add(dealtCard.gameObject);
    }

    public IEnumerator HeroAttackPhase()
    {
        float combatDelay = 1f;
        yield return new WaitForSeconds(combatDelay);
        turnPhase = phase.CombatPhase;

        for (int i = 0; i < attackers.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
            defenderHeroObject = liveHeroes[0];
            animationController.AttackStart(attackers[i], defenderHeroObject);
            defHero.heroCardData.hp -= attacker.creatureCardData.dmg; //creature deals damage
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
        print("You Lose");
    }

    public void Victory() //function for winning the game
    {
        print("You Win");
    }
}