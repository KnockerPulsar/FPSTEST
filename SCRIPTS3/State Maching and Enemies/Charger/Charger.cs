using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Responsible for maintaing the state machine for the enemy, the states and their transitions.
public class Charger : BaseAIStateMachine
{
    //A container responsible for storing commonly used / shared variables.

    [Header("Idle")]
    public AnimationCurve idleFloatingCurve;                //The Y axis curve responsible for animating the enemy while idle.                
    public float visionConeAngle = 40f;                     //The horizontal FOV of the enemy.
    public float range = 60f;                               //The range at which the enemy starts chasing the player.

    [Header("Rotation")]
    public float rotationSpeed = 4f;                        //How fast the enemy rotates towards the player.
    public float chargeAngle = 3f;                          //The angle difference bettween the enemy's forward vector and the enemy-player vector at which the enemy starts charging.

    [Header("Charge")]
    public ParticleSystem chargePS;                         //The particle system shown when charging.
    public float chargeMovementSpeed = 50f;                 //Movement speed while charging.
    public float chargeLength = 1f;                         //The length of the charge in seconds.
    public float checkFwdDist = 1f;                         //How far forward the enemy checks for floor in meters.
    public float damage = 10f;                              //The damage done when the player is hit.


    private StateMachine stateMachine;                      //The state machine responsible for maintaining the different states/behaviours of the enemy.
    private BasicMeleeIdleState idleState;                  //The idle behaviour of the enemy. Ticks only when the player is out of range/not seen by the enemy.
    private ChargeRotation rotationState;                   //The rotation behaviour of the enemy. Ticks only when the enemy is not (almost) facing the player but is within range.
    private ChargeAttack chargeAttackState;                 //The charge behaviour of the enemy. Ticks after the enemy faces the player and while the enemy is above a floor or within the charge duration.
    private Rigidbody RB;                                   //The rigid body of the enemy, used to move the enemy.


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        //Starts by setting up the state machine and needed states along with other variables.
        RB = GetComponent<Rigidbody>();
        chargePS.Stop();
        stateMachine = new StateMachine();
        idleState = new BasicMeleeIdleState(gameObject, visionConeAngle, range, idleFloatingCurve);
        rotationState = new ChargeRotation(gameObject, rotationSpeed, chargeAngle, RB);
        chargeAttackState = new ChargeAttack(gameObject, chargePS, chargeMovementSpeed, chargeLength, checkFwdDist, RB);

        //Setting up the transitions
        AT(idleState, rotationState, PlayerDetected());
        AT(rotationState, idleState, PlayerUnDetected());
        AT(rotationState, chargeAttackState, StartCharge());
        AT(chargeAttackState, rotationState, EndCharge());


        //Assigning the initial state of the state machine.
        stateMachine.SetState(idleState);

        //Converting the given angle from degrees to radians and calculating the cosine of that as the dot product produces the cos(angle in radians).
        visionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);

        //A wrapper function to make the transition setup cleaner.
        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        //The conditions for each state transition.
        Func<bool> PlayerDetected() => () => (idleState.sawPlayer);
        Func<bool> PlayerUnDetected() => () => rotationState.playerDistance > range;
        Func<bool> StartCharge() => () => rotationState.readyToCharge;
        Func<bool> EndCharge() => () => chargeAttackState.chargeDone;

    }

    // Update is called once per frame
    //Tick the currentState and ONLY the current state. Other states don't tick unless they're pickedUp.
    void Update() => stateMachine.Tick();


    //Damages the player on entering the damage trigger while the enemy is charging.
    new private void OnTriggerEnter(Collider other)
    {
        if (stateMachine.currentState == chargeAttackState)
        {
            PlayerHealthManager playerHM = other.gameObject.GetComponent<PlayerHealthManager>();
            if (playerHM != null)
            {
                PlayerVariables.playerRB.AddForce(Vector3.up * damage * 200f);
                playerHM.RecieveDamage(damage);

            }
        }
    }
}

