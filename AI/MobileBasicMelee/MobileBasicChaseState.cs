using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//Chase state: randomly dashes forward + left/right while going towards the player.
public class MobileBasicMeleeChaseState : StateInterface
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

    float dashChance = 0.2f;                        //The chance the enemy will dash.
    float dashCooldown = 1f;                        //The length of time between dashes.
    float dashLength = 0.2f;                        //The length of time the enemy will dash before continuing its movement.

    float rand;                                     //A variable used to store a random number that is generated each "dashCooldown" seconds.
    float nextDashTime = 0;                         //A variable used to store the next time the enemy is supposed to dash.
    Vector3 dashVector;                             //A vector used to store the dash direction.
    float moveTimer = 0;                            //A vector used to store the next time the enemy is supposed to continue it's movement.
    float dir;                                      //A float that stores a random number between -1 and 1 which determines whether the enemy will dash forward, right or left.
    float heightOffset = 2f;
    RaycastHit[] heightOffsetHits;


    //How the climb check works:
    //If the enemy's velocity is low enough, it checks if something is in front of the enemy.
    //If something is there, it checks the top, if there is no object there, this means that the enemy is near small ledge.
    //So it sets the enemy's velocity to be vertical so it climbs whatever is in front of it.
    //The jumpCheckTop can be extended so that the enemy checks higher or lower.

    public MobileBasicMeleeChaseState(GameObject inputEntity, float inputMS, float inputSD, float inputJCI, float inputDashChance, float inputDashCooldown, float inputDashLength, Rigidbody inputRB, GameObject inputCheckObj, PlayerVariables inputPvars)
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

        //If the enemy can't climb, checks if the enemy is supposed to dash or move.
        else if (!canClimb)
        {
            //If the enemy is supposed to dash, generates a random number and checks if it's larger than the dash chance.
            //If so, generates another random number and determines whether the enemy will dash forwards, right or left.
            //It also updates the counters so that it can't dash until after "dashCooldown" and can't move until "dashLength" after dashing.
            //Had to do this because setting the velocity nullifies the effect of the dash, I think I can use Rigidbody.AddForce() + a velocity limiter to alleviate this.
            if (Time.time >= nextDashTime)
            {
                dashVector = Vector3.zero;
                rand = Random.Range(0, 1f);
                if (rand > dashChance)
                {
                    dir = Mathf.Round(Random.Range(-1, 2));
                    if (dir == 0)
                    {
                        dashVector += entity.transform.forward * 100f;
                    }
                    else
                        dashVector += entity.transform.right * dir * 100f;

                    RB.velocity += dashVector;

                    nextDashTime = Time.time + dashCooldown;
                    moveTimer = Time.time + dashLength;
                }
            }

            //Movement as long as the enemy is far enough.
            if (Time.time >= moveTimer)
            {
                if (playerDistance > stoppingDist)
                {
                    RB.velocity = (enemyPlayerVec * movementSpeed);

                    //Applies gravity if the enemy is too far high off the ground.
                    if (!Physics.Raycast(entity.transform.position, -entity.transform.up, heightOffset))
                        RB.velocity += Physics.gravity / 10f;
                }

                else
                    RB.velocity = Vector3.zero;
            }
        }

        //Otherwise, stops the enemy for the attack state.
        else if (playerDistance <= stoppingDist)
            RB.velocity = Vector3.zero;
    }

    public void OnEnter()
    {
        dashChance = 1 - dashChance;
        moveTimer = Time.time;
        nextDashTime = Time.time + dashCooldown;
    }

    public void OnExit() { }

}
