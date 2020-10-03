using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : StateInterface
{

    public bool chargeDone = false;             //Used for state transitions, indicates whether the charge is done (time / no floor).

    //Variables passed from the state machine.
    GameObject entity;                          //A reference the the root gameObject of the enemy (contains the body, particle systems and other game objects).
    ParticleSystem PS;                          //The particle system shown when charging.
    float chargeMS = 50f;                       //Movement speed while charging.
    float chargeLength = 1f;                    //The length of the charge in seconds.
    float checkFwdDist = 1f;                    //How far forward the enemy checks for floor in meters.
    Rigidbody RB;                               //The rigid body used to set velocity.

    //Internal variables
    float chargeEndTime;                        //When the charge should end, equals the charge start time + the charge length.
    bool noFloor;                               //Stores the result of the floor check.

    public ChargeAttack(GameObject inputEntity, ParticleSystem inputPS, float inputCMS, float inputCL, float inputCFD, Rigidbody inputRB)
    {
        entity = inputEntity;
        PS = inputPS;
        chargeMS = inputCMS;
        chargeLength = inputCL;
        checkFwdDist = inputCFD;
        RB = inputRB;
    }

    public void Tick()
    {
        //Checks if there's no floor below the enemy.
        noFloor = !Physics.Raycast(entity.transform.position + entity.transform.forward * checkFwdDist, -Vector3.up, 100f);

        //Stops charging if the time is up or there's no floor.
        if (Time.time >= chargeEndTime || noFloor)
        {
            RB.velocity = Vector3.zero;
            chargeDone = true;
        }
        //Continues charging otherwise.
        else
            RB.velocity = entity.transform.forward * chargeMS;

    }

    public void OnEnter()
    {
        PS.Play();
        chargeDone = false;
        chargeEndTime = Time.time + chargeLength;
    }

    public void OnExit()
    {
        PS.Stop();
    }
}
