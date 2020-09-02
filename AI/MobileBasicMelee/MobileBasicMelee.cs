using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Responsible for maintaing the state machine for the enemy (BasicMelee for now), the states and their transitions.
public class MobileBasicMelee : MonoBehaviour
{
    public PlayerVariables pVars;                           //A container responsible for storing commonly used / shared variables.

    [Header("Idle")]
    public AnimationCurve idleFloatingCurve;                //The Y axis curve responsible for animating the enemy while idle.                
    public float visionConeAngle;                           //The horizontal FOV of the enemy.
    public float range;                                     //The range at which the enemy starts chasing the player.

    [Header("Chase")]
    public GameObject checkObj;                             //The object from where the bottom climb check originates.
    public float movementSpeed;                             //The movement speed of the enemy.
    public float jumpCheckIterations;                       //Aprroximately how many meters up the jumpCheckTop should originate from. Passed on from the main script.
    public float stoppingDist;                              //The distance at which the enemy starts attacking the player.
    public float dashChance = 0.2f;                         //The chance the enemy will dash.
    public float dashCooldown = 1f;                         //The length of time between dashes.
    public float dashLength = 0.2f;                         //The length of time the enemy will dash before continuing its movement.

    [Header("Attack")]
    public ParticleSystem attackPS;                         //A particle system used as visual feedback to indicate that the enemy is attacking.
    public GameObject smile;                                //The smile indicating that the enemy is not in the attack state.
    public GameObject poker;                                //The poker face indicating that the enemy is in the attack state.
    public float beforeAttackDelay = 0;                     //The initial delay before the attack rotation starts.
    public float rotationDuration = 0.25f;                  //The length of the rotation animation.
    public float particleDelay = 0.175f;                    //How long after finishing the rotation should the particle effect be visible for?
    public float afterAttackDelay = 1f;                     //The delay after the attack before re-entering the state again or switching to another state.

    private StateMachine stateMachine;                      //The state machine responsible for maintaining the different states/behaviours of the enemy.
    private BasicMeleeIdleState idleState;                  //The idle behaviour of the enemy. Ticks only when the player is out of range/not seen by the enemy.
    private MobileBasicMeleeChaseState chaseState;          //The chase behaviour of the enemy. Ticks when the player is in range but not in stoppingDist.
    private BasicMeleeAttackState attackState;              //The attack behaviour of the enemy. Ticks when the player is closer than the stopping distance.
    private Rigidbody RB;                                   //The rigid body of the enemy, used to move the enemy.


    // Start is called before the first frame update
    void Start()
    {
        //Starts by setting up the state machine and needed states along with other variables.
        RB = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        idleState = new BasicMeleeIdleState(gameObject, idleFloatingCurve, pVars);
        chaseState = new MobileBasicMeleeChaseState(gameObject, movementSpeed, stoppingDist, jumpCheckIterations, dashChance, dashCooldown, dashLength, RB, checkObj, pVars);
        attackState = new BasicMeleeAttackState(beforeAttackDelay, rotationDuration, particleDelay, afterAttackDelay, gameObject, RB, smile, poker, attackPS, pVars);

        //Setting up the transitions
        AT(idleState, chaseState, PlayerDetected());        //idleState -> chaseState
        AT(chaseState, idleState, PlayerUnDetected());      //chaseState -> idleState
        AT(chaseState, attackState, InAttackRange());       //chaseState -> attackState
        AT(attackState, chaseState, HitPlayer());           //attackState -> chaseState

        //Assigning the initial state of the state machine.
        stateMachine.SetState(idleState);

        //Converting the given angle from degrees to radians and calculating the cosine of that as the dot product produces the cos(angle in radians).
        visionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);

        //A wrapper function to make the transition setup cleaner.
        void AT(StateInterface from, StateInterface to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        //The conditions for each state transition.
        Func<bool> PlayerDetected() => () => (idleState.playerAngle >= visionConeAngle && idleState.playerDistance <= range);
        Func<bool> PlayerUnDetected() => () => chaseState.playerDistance > range;
        Func<bool> InAttackRange() => () => Vector3.Distance(pVars.player.transform.position, transform.position) <= 5f && !attackState.attackFinished;
        Func<bool> HitPlayer() => () => attackState.attackFinished;
    }

    // Update is called once per frame
    //Tick the currentState and ONLY the current state. Other states don't tick unless they're active.
    void Update() => stateMachine.Tick();
}
