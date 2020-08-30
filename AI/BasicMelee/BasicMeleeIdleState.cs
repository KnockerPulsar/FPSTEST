using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Responsible for providing visual feedback that the enemy is idle and checking whether the player has come in vision and in range.
public class IdleState : StateInterface
{
    private PlayerVariables pVars;                  //A container responsible for storing commonly used / shared variables. Passed on from the main script.
    private AnimationCurve floatCurve;              //The curve the enemy's y position should follow. 
    private GameObject entity;                      //A reference to the enemy's object.
    private Vector3 enemyPlayerVec;                 //A vector starting at the enemy and ending at the player. used to check if the player has come into view.
    private Vector3 playerHorzPos;                  //Where the player is in terms of X and Z coordinates.
    public float playerAngle;                       //The angle between the front vector of the enemy and the player.
    public float playerDistance;                    //The distance between the enemy and the player.

    public IdleState(GameObject parentEntitiy, AnimationCurve idleFloatingCurve, PlayerVariables inputpVars)
    {
        entity = parentEntitiy;
        floatCurve = idleFloatingCurve;
        pVars = inputpVars;
    }

    public void Tick()
    {
        //Calculating where the player is in terms of X and Z coordinates.
        playerHorzPos = pVars.player.transform.position;
        playerHorzPos.y = entity.transform.position.y;

        //Calculating the vector starting at the enemy and facing the player.
        enemyPlayerVec = playerHorzPos - entity.transform.position;
        enemyPlayerVec.Normalize();

        //Calculating the cosine of the angle between the forward vector of the enemy and the enemyPlayerVec vector, along with the distance between the enemy and the player.
        playerAngle = Vector3.Dot(enemyPlayerVec, entity.transform.forward);
        playerDistance = Vector3.Distance(pVars.player.transform.position, entity.transform.position);

        //Floating the enemy in place.
        entity.transform.position += new Vector3(0, floatCurve.Evaluate(Time.time) / 200f, 0);
    }

    public void OnEnter() { }

    public void OnExit() { }
}
