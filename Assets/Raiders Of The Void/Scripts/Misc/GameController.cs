using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum turn {Player1, Player2}; //List of states
    public enum phase {MainPhase, CombatPhase}; //List of states

    public turn turnState; //State for current player turn

    public phase turnPhase; //State for current game phase

    public int p1HP = 99, defenderHeroHP, turnCount = 3;

    public GameObject P1Hero;

    public Text p1HealthText; //UI text

    public Transform p1BattleTransform, p2BattleTransform, battleTransform; //Board areas for each players cards

    public List<CreatureCardData> p1Deck, p2Deck, deckList; //lists for the carddata of each decks and hand

    public List<GameObject> p1LiveMonsters, p2LiveMonsters, deadCreatures, attackers, defenders; //lists for the card prefabs on the board and dead cards

    public CreatureCardUI creatureCardTemplate; //card prefab

    public AnimationController animationController;

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
            deckList = p2Deck;
            attackers = p2LiveMonsters;
            defenders = p1LiveMonsters;
            
            if (deckList.Count > 0)
            {
                topDeckCard = p2Deck[0];
            }
            else
            {
                topDeckCard = null;
            }

        }
        else if (turnState == turn.Player2) //checks if currently Player 2's turn
        {
            turnState = turn.Player1;
            attackers = p1LiveMonsters;
            defenders = p2LiveMonsters;
        }

        if (deckList.Count > 0 & turnState == turn.Player2)
        {
            TurnStart(topDeckCard, deckList, attackers, defenders, defenderHeroHP);
        }
        else
        {
            TurnStart(null, deckList, attackers, defenders, defenderHeroHP);
        }
    }

    void TurnStart(CreatureCardData topDeckCard, List<CreatureCardData> deckList, List<GameObject> attackers, List<GameObject> defenders, int defenderHeroHP)
    {
        if (attackers.Count == 0 & turnState == turn.Player2) //checks if deck has cards left
        {
            DealHand();
            StartCoroutine(HeroAttackPhase(p2LiveMonsters, p1LiveMonsters, p1HP));
        }

        else if (turnState == turn.Player2)
        {
            StartCoroutine(HeroAttackPhase(p2LiveMonsters, p1LiveMonsters, p1HP));
        }

        if (turnState == turn.Player1 & turnCount > 1)
        {
            turnCount--;
            TurnSwap();
        }
    }

    public void DealHand()
    {
        DealCard(topDeckCard, deckList, battleTransform);
        DealCard(topDeckCard, deckList, battleTransform);
        DealCard(topDeckCard, deckList, battleTransform);
        DealCard(topDeckCard, deckList, battleTransform);
    }

    public void DealCard(CreatureCardData topDeckCard, List<CreatureCardData> deckList, Transform battleTransform) //deals card to Player
    {
        CreatureCardData card = Instantiate(topDeckCard); //instantiates an instance of the carddata scriptable object
        CreatureCardUI dealtCard = Instantiate(creatureCardTemplate); //instantiates an instance of the card prefab
        dealtCard.transform.SetParent(battleTransform.transform, false); //moves card to handzone
        dealtCard.creatureCardData = card; //sets the cards data to the card dealt
        deckList.Remove(topDeckCard); //removes card from deck list
        attackers.Add(dealtCard.gameObject);
    }

    //public void PlayCard(GameObject playedCard, CreatureCardData monsterCardData , Button button, AudioSource audioSource) //plays a clicked card to the baord
    //{
    //    button.interactable = false; //disables the play card button
    //    playedCard.transform.SetParent(battleTransform.transform, false); //moves the card to the battlezone
    //    attackers.Add(playedCard); //adds the card to p1liveMonsters list
    //    DeckUIUpdate();
    //    StartCoroutine(CombatPhase(p1LiveMonsters, p2LiveMonsters, p2HP)); //calls AttackRound function and passes it argument parameters
    //}

    //public void P2PlayCard() //AI plays a random card
    //{
    //    int rng = Random.Range(0, p2Hand.Count); //generates random number
    //    //MonsterCardUI.PlayClickedCard(monsterCard);
    //    MonsterCardData card = p2Hand[rng]; //assigns the random number to the card to be played.
    //    MonsterCardUI playedCard = Instantiate(monsterCardTemplate); //instantiates an instance of the card
    //    playedCard.monsterCardData = (MonsterCardData)card; //sets the cards data to the card played from hand
    //    playedCard.button.interactable = false; //disables the play card button
    //    playedCard.transform.SetParent(battleTransform.transform, false); //moves card to battlezone
    //    p2LiveMonsters.Add(playedCard.gameObject); //adds the card to p2livemonsters list
    //    p2Hand.Remove(card); //removes the card from hand list
    //    DeckUIUpdate();
    //    StartCoroutine(CombatPhase(p2LiveMonsters, p1LiveMonsters, p1HP)); //calls attack function
    //}

    public IEnumerator HeroAttackPhase(List<GameObject> attackers, List<GameObject> defenders, int defenderHeroHP)
    {
        float endTurnDelay = 1.75f;
        float combatDelay = 1f;
        yield return new WaitForSeconds(combatDelay);
        turnPhase = phase.CombatPhase;
        if (attackers.Count > defenders.Count) //checks if attack has more monsters than defender
        {
            for (int i = defenders.Count; i < attackers.Count; i++) //loop repeats for each creature fighting the hero
            {
                CreatureCardUI attacker = attackers[i].GetComponent<CreatureCardUI>();
                animationController.AttackStart(attackers[i], P1Hero);
                defenderHeroHP -= attacker.creatureCardData.attack;
            }
        }
        HeroHPUIUpdate();

        if (defenderHeroHP <= 0) //checks if hero is dead
        {
            print("GameOver by 0 HP");
            GameOver();
            CancelInvoke();
            //return;
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

    //public void Draw()
    //{
    //    GameOverObject.SetActive(true);
    //    gameoverText.text = "Game Over, Tied Game";
    //}
}