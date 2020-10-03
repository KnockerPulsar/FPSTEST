using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for slowly rotating the enemy towards the player.
//Switches to the charge state after a short delay.
public class ChargeRotation : StateInterface
{
    //Used to tranistion to the idle or charge states.
    public float playerDistance;                    //Stores the distance between the player and the enemy.
    public bool readyToCharge = false;              //if the enemy is (almost) looking at the player, goes into the charge state.

    //Variables passed from the state machine.
    GameObject entity;                              //A reference the the root gameObject of the enemy (contains the body, particle systems and other game objects).
    float rotationSpeed = 3f;                       //How fast the enemy rotates towards the player.
    float chargeAngle = 3f;                         //The angle difference bettween the enemy's forward vector and the enemy-player vector at which the enemy starts charging.
    Rigidbody RB;                                   //The rigid body used to drop the enemy when it's high enough off the floor. (psuedo-gravity?)
    PlayerVariables pVars;                          //A container responsible for storing commonly used / shared variables.

    //Internal variables.
    float floorOffset = 1f;                         //How far off the floor the enemy should be.                         
    Vector3 hPlayerEnemyVec;                        //A horizontal vector originating at the enemy and pointing towards the player.
    float dot;                                      //The result of the player-enemy vector and the enemy's forward vector's dot product.
    RaycastHit[] floorOffsetCheck;                  //Used to store the result of the floor offset ray casts.


    public ChargeRotation(GameObject inputEntity, float inputRotSpeed, float inputChargeAngle, Rigidbody inputRB, PlayerVariables inputPVars)
    {
        entity = inputEntity;
        rotationSpeed = inputRotSpeed;

        //Gets the charge angle from the player, converts it into radians and calculates the cosine as the dot product returns the cosine too. 
        chargeAngle = inputChargeAngle;
        chargeAngle = Mathf.Cos(chargeAngle * Mathf.Deg2Rad);

        RB = inputRB;
        pVars = inputPVars;
    }

    public void Tick()
    {
        //Calculates the distance to the player.
        playerDistance = Vector3.Distance(pVars.player.transform.position, entity.transform.position);

        //Calculates the horizontal vector between the enemy and the player, then normalizes it.
        hPlayerEnemyVec = pVars.player.transform.position - entity.transform.position;
        hPlayerEnemyVec.y = 0;
        hPlayerEnemyVec.Normalize();

        //Rotates the enemy towards the player.
        entity.transform.forward = Vector3.Slerp(entity.transform.forward, hPlayerEnemyVec, Time.deltaTime * rotationSpeed);

        //Checks if the enemy is too high off the floor and drops a bit down.
        floorOffsetCheck = Physics.RaycastAll(entity.transform.position, -Vector3.up * 100f);
        if (floorOffsetCheck.Length > 0 && floorOffsetCheck[0].distance > floorOffset)
            RB.velocity += Physics.gravity / 1000f;

        //Checks if the enemy is (almost) facing the player and changes states if so.
        dot = Vector3.Dot(entity.transform.forward, hPlayerEnemyVec);
        if (Mathf.Abs(dot) > chargeAngle)
            readyToCharge = true;
    }

    public void OnEnter() { readyToCharge = false; }

    public void OnExit() { }
}
