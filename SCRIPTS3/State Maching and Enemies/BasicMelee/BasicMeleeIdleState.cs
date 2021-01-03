using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

//Responsible for providing visual feedback that the enemy is idle and checking whether the player has come in vision and in range.
public class BasicMeleeIdleState : IState
{
    private AnimationCurve floatCurve;              //The curve the enemy's y position should follow. 
    private GameObject entity;                      //A reference to the enemy's object.
    private Vector3 enemyPlayerVec;                 //A vector starting at the enemy and ending at the player. used to check if the player has come into view.
    public float playerAngle;                       //The angle between the front vector of the enemy and the player.
    public float playerDistance;                    //The distance between the enemy and the player.

    public float visionConeAngle;
    public float range;
    public bool sawPlayer;

    public BasicMeleeIdleState(GameObject parentEntitiy, float inputVisionCone, float inputRange, AnimationCurve idleFloatingCurve)
    {
        entity = parentEntitiy;
        visionConeAngle = inputVisionCone;
        floatCurve = idleFloatingCurve;
        range = inputRange;
        sawPlayer = false;
    }

    public void Tick()
    {
        //Calculating where the player is in terms of X and Z coordinates.
        //playerHorzPos = PlayerVariables.player.transform.position;
        //playerHorzPos.y = entity.transform.position.y;

        //Calculating the vector starting at the enemy and facing the player.
        enemyPlayerVec = PlayerVariables.player.transform.position - entity.transform.position;
        enemyPlayerVec.Normalize();

        //Calculating the cosine of the angle between the forward vector of the enemy and the enemyPlayerVec vector, along with the distance between the enemy and the player.
        playerAngle = Vector3.Dot(enemyPlayerVec, entity.transform.forward);
        playerDistance = Vector3.Distance(PlayerVariables.player.transform.position, entity.transform.position);

        //Floating the enemy in place.
        entity.transform.position += new Vector3(0, floatCurve.Evaluate(Time.time) / 200f, 0);

        if (playerAngle >= visionConeAngle && playerDistance < range)
        {
            RaycastHit hit;
            Physics.Raycast(entity.transform.position, enemyPlayerVec, out hit, range);
            Debug.DrawRay(entity.transform.position, enemyPlayerVec * range, Color.red, 0.1f);


            if (hit.transform.CompareTag("Player"))
                sawPlayer = true;
        }

    }

    public void OnEnter() { sawPlayer = false; }

    public void OnExit() { }
}
