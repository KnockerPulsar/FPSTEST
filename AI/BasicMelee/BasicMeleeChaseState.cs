using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Responsible for following after the player if in range.
public class BasicMeleeChaseState : StateInterface
{
    GameObject entity;                              //A reference to the enemy object, used for checking the position and doing raycasts. Passed on from the main script.
    float movementSpeed;                            //The movement speed of the enemy. Passed on from the main script.
    float stoppingDist;                             //The stopping distance of the enemy. Passed on from the main script.
    Rigidbody RB;                                   //The rigid body of the enemy. Used for moving the enemy. Passed on from the main script
    GameObject checkObj;                            //The object at which the jumpCheckBottomOriginates. Passed on from the main script.
    PlayerVariables pVars;                          //A container responsible for storing commonly used / shared variables. Passed on from the main script.
    float jumpCheckIterations;                      //Aprroximately how many meters up the jumpCheckTop should originate from. Passed on from the main script.

    public float playerDistance;                    //The distance between the enemy and player. Used to enter the attack state.
    Vector3 enemyPlayerVec;                         //The normalized vector originating at the enemy and ending at the player. Used for moving the enemy and rotating it.
    RaycastHit[] jumpCheckBottom;                   //Checks if something is in front of the enemy at the bottom. Used for climbing.
    RaycastHit[] jumpCheckTop;                      //Same as jumpCheckBottom but for the top.
    float HVelocityMag = 0;                         //A variable used to store the horizontal velocity of the player, Used for climbing.
    bool canClimb;                                  //A variable used to indicate that the enemy should climb up.
    float heightOffset = 1.5f;

    //How the climb check works:
    //If the enemy's velocity is low enough, it checks if something is in front of the enemy.
    //If something is there, it checks the top, if there is no object there, this means that the enemy is near small ledge.
    //So it sets the enemy's velocity to be vertical so it climbs whatever is in front of it.
    //The jumpCheckTop can be extended so that the enemy checks higher or lower.

    public BasicMeleeChaseState(GameObject inputEntity, float inputMS, float inputSD, float inputJCI, Rigidbody inputRB, GameObject inputCheckObj, PlayerVariables inputPvars)
    {
        pVars = inputPvars;
        entity = inputEntity;
        stoppingDist = inputSD;
        movementSpeed = inputMS;
        RB = inputRB;
        checkObj = inputCheckObj;
        jumpCheckIterations = inputJCI;
    }


    public void Tick()
    {
        //Calculating the horizontal enemy-player vector and normalizing it.
        enemyPlayerVec = pVars.player.transform.position - entity.transform.position;
        enemyPlayerVec.y = 0;
        enemyPlayerVec.Normalize();

        //Calculating the distance between the player and the enemy.
        //Note that this isn't horizontal distance.
        playerDistance = Vector3.Distance(pVars.player.transform.position, entity.transform.position);

        //Rotates the enemy towards the player smoothly.
        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, Quaternion.LookRotation(enemyPlayerVec, Vector3.up), Time.deltaTime * 2000f);

        //Climb checking by first calculating the horizontal velocity.
        canClimb = false;
        HVelocityMag = Mathf.Sqrt(RB.velocity.x * RB.velocity.x + RB.velocity.z * RB.velocity.z);

        //If it's low enough, does the bottom raycast.
        if (HVelocityMag < 1)
        {
            jumpCheckBottom = Physics.RaycastAll(checkObj.transform.position - 0.05f * Vector3.up, entity.transform.forward, 2f);
            Debug.DrawRay(checkObj.transform.position, entity.transform.forward * 2, Color.red, 3f);

            //If the cast hits something that's not the player, proceeds to do the top check.
            if (jumpCheckBottom.Length > 0 && !jumpCheckBottom[0].collider.CompareTag("Player"))
            {
                //Starting from where the center of the enemy is, a ray is cast and checks if it hit nothing or if what was hit is a player.
                //If so, sets the boolean and breaks out of the loop.
                //Otherwise, continues until the last iteration.
                for (int i = 0; i < jumpCheckIterations; i++)
                {
                    jumpCheckTop = Physics.RaycastAll(entity.transform.position + Vector3.up * i, entity.transform.forward, 2f);
                    Debug.DrawRay(entity.transform.position + Vector3.up * i, entity.transform.forward * 2, Color.red, 3f);

                    if (jumpCheckTop.Length == 0 || jumpCheckTop.Length > 0 && jumpCheckTop[0].collider.CompareTag("Player"))
                    {
                        canClimb = true;
                        break;
                    }
                    else
                        continue;
                }
            }
        }

        //If the enemy should climb, sets the vertical velocity.
        if (canClimb)
            RB.velocity = Vector3.up * Time.deltaTime * 1000f;

        //If not and the player is far enough, continues following.
        if (!canClimb && playerDistance > stoppingDist)
        {
            RB.velocity = (enemyPlayerVec * movementSpeed);

            //Applies gravity if the enemy is too far high off the ground.
            if (!Physics.Raycast(entity.transform.position, -entity.transform.up, heightOffset))
                RB.velocity += Physics.gravity;
        }

        //Otherwise, stops the enemy for the attack state.
        else if (playerDistance <= stoppingDist)
            RB.velocity = Vector3.zero;
    }

    public void OnEnter() { }

    public void OnExit() { }
}
