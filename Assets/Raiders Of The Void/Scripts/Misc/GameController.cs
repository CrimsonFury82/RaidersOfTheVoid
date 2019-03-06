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

    int p1HP, turnsRemaining, AP;

    public GameObject defenderHero;

    public GameObject heroTarget;

    public Button endTurnButton;

    public Transform p2BattleTransform, heroTransform; //Board areas for each players cards

    public List<CreatureCardData> aiDeck; //Deck list

    public List<HeroCardData> heroDeck; //Deck list

    public List<GameObject> attackers, liveHeroes; //lists for the card prefabs on the board and dead cards

    public CreatureCardUI creatureCardTemplate; //card prefab

    public HeroCardUI heroCardTemplate;

    public AnimationController animationController;

    float endTurnDelay = 1.75f;

    CreatureCardUI creatureCardUI; //script

    CreatureCardData topDeckCard;

    HeroCardUI heroCard;

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
        DealHero(heroDeck[0], heroTransform);
        DealHand();
        turnState = turn.Player1;
        APUpdate();
    }

    public void TurnUpkeep() //function for Start of turn
    {
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
        DealCard();
        DealCard();
        DealCard();
        DealCard();
    }

    public void WeaponAttack(GameObject playedCard, WeaponCardData weaponCardData, Button button)
    {
        if(AP - weaponCardData.ap <0)
        {
            print("Not enough AP");
        }
        else { 
            print("DMG = " + weaponCardData.dmg + " AP = " + weaponCardData.ap);
            AP -= weaponCardData.ap;
            apText.text = "AP = " + AP.ToString();
            print("Select target");
            //insert target selection code here
        }
    }

    public void DealHero(HeroCardData hero, Transform heroTransform) //Deals hero cards at start of game
    {
        HeroCardData card = Instantiate(hero);
        HeroCardUI tempCard = Instantiate(heroCardTemplate); //instantiates an instance of the card prefab
        tempCard.transform.SetParent(heroTransform.transform, false); //moves card 
        tempCard.heroCardData = card;
        liveHeroes.Add(tempCard.gameObject);
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
        dealtCard.transform.SetParent(p2BattleTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        aiDeck.Remove(topDeckCard); //removes card from deck list
        attackers.Add(dealtCard.gameObject);
    }

    public IEnumerator HeroAttackPhase()
    {
        float combatDelay = 1f;
        yield return new WaitForSeconds(combatDelay);
        turnPhase = phase.CombatPhase;

        HeroCardUI defHero = defenderHero.GetComponent<HeroCardUI>();

        for (int i = 0; i < attackers.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
            heroTarget = liveHeroes[0];
            animationController.AttackStart(attackers[i], heroTarget);
            
            //The defhero line is bugged and not working. Need to work through code and debug

            //print("hero hp = " + defHero.heroCardData.hp);
            //defHero.heroCardData.hp -= attacker.creatureCardData.dmg; //creature deals damage
            //defHero.hpText.text = defHero.heroCardData.hp.ToString(); //updates UI HP text
            //p1HP = defHero.heroCardData.hp;
        }

        if (p1HP <= 0) //checks if hero is dead
        {
            print("GameOver by 0 HP");
            GameOver();
            //CancelInvoke();
        }
        Invoke("TurnUpkeep", endTurnDelay);
    }

    public void GameOver() //function for when the game has ended
    {
        
    }
}