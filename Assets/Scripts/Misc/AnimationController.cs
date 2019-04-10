using UnityEngine;
using System.Collections;

//Script written by Aston Olsen

public class AnimationController : MonoBehaviour
{
    public GameController gameController;

    public IEnumerator AttackMove(GameObject attacker, GameObject defender) //function for animating movement of attacking card
    {
        bool attacked = false;
        Vector3 startPos, targetPos; //starting position of creature and target position (hero card location)
        startPos = attacker.transform.position; //stores vector of this cards starting position 
        targetPos = defender.transform.position; //stores vector of this cards target position
        float animationLength = 0.8f; //length of animation
        float moveElapsed = 0f; //amount of time passed moving toward target
        float returnElapsed = 0f; //amount of time passed returning to orignal position
        float speed = 10f; //speed cards move during animation

        if (gameController.turnPhase == GameController.phase.CombatPhase & attacker != null & defender != null) //checks it is combat phase and attacker and defender exsist
        {
            while (moveElapsed < animationLength) //checks if time passed is less than animation length
            {
                moveElapsed = Mathf.Min(moveElapsed + (Time.deltaTime / animationLength), animationLength);
                attacker.transform.position = Vector3.Lerp(startPos, targetPos, moveElapsed*speed);
                //print("MoveE " + moveElapsed);
                //print("ReturnE " + moveElapsed);
                yield return null;
            }
            attacked = true;
        }

        if (gameController.turnPhase == GameController.phase.CombatPhase & attacker != null & defender != null & attacked == true) //checks it is combat phase and attacker and defender exsist and creature has attacked
        {
            while (returnElapsed < animationLength & moveElapsed >= animationLength)
            {
                returnElapsed = Mathf.Min(returnElapsed + (Time.deltaTime / animationLength), animationLength);
                attacker.transform.position = Vector3.Lerp(targetPos, startPos, returnElapsed * speed);
                //print("MoveE " + moveElapsed);
                //print("ReturnE " + moveElapsed);
                yield return null;
            }
        }
    }
}