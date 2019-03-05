using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    public int p1HP, turnsRemaining;

    public GameObject P1Hero;

    public Text p1HealthText; //UI text

    public Transform p2BattleTransform; //Board areas for each players cards

    public List<CreatureCardData> aiDeck; //AI deck list list

    public List<GameObject> attackers; //lists for the card prefabs on the board and dead cards

    public CreatureCardUI creatureCardTemplate; //card prefab

    public AnimationController animationController;

    float endTurnDelay = 1.75f;

    CreatureCardUI creatureCardUI; //script

    CreatureCardData topDeckCard;
            
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
        HeroHPUIUpdate(); //updates UI text
        turnState = turn.Player1;
        TurnSwap();
    }

    void HeroHPUIUpdate() //updates Hero HP UI text
    {
        p1HealthText.text = p1HP.ToString();
    }
   
    public void TurnSwap() //function for Start of turn
    {
        turnPhase = phase.MainPhase;

        if (turnState == turn.Player1) //checks if currently Player 1's turn
        {
            turnState = turn.Player2;
        }
        else if (turnState == turn.Player2) //checks if currently Player 2's turn
        {
            turnState = turn.Player1;
        }

        TurnStart();
    }

    void TurnStart()
    {
        if (turnState == turn.Player2) //checks if it is AI's turn
        {
            if(attackers.Count == 0) //checks if AI has creatures on the board
            {
                DealHand();
            }
            //StartCoroutine(HeroAttackPhase()); //starts attack phase
        }

        if (turnState == turn.Player1 & turnsRemaining > 1)
        {
            turnsRemaining--;
            Invoke("TurnSwap", endTurnDelay);
        }
    }

    public void DealHand()
    {
        DealCard();
        DealCard();
        DealCard();
        DealCard();
        DealCard();
        DealCard();
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
      
        for (int i = 0; i < attackers.Count; i++) //loop repeats for each creature fighting the hero
        {
            CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
            animationController.AttackStart(attackers[i], P1Hero);
            p1HP -= attacker.creatureCardData.dmg;
        }
       
        HeroHPUIUpdate();

        if (p1HP <= 0) //checks if hero is dead
        {
            print("GameOver by 0 HP");
            GameOver();
            CancelInvoke();
        }
        Invoke("TurnSwap", endTurnDelay);
    }

    public void GameOver() //function for when the game has ended
    {
        print("Game Over");
    }

    //public void P1Wins()
    //{
    //    GameOverObject.SetActive(true);
    //    gameoverText.text = "Game Over, Player1 Wins";
    //}

    //public void P2Wins()
    //{
    //    GameOverObject.SetActive(true);
    //    gameoverText.text = "Game Over, Player2 Wins";
    //}
}