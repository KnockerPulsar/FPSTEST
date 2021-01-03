using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Responsible for maintaing the state machine for the enemy, the states and their transitions.
public class BasicRanged : BaseAIStateMachine
{

    [Header("Idle")]
    public AnimationCurve idleFloatingCurve;                //The Y axis curve responsible for animating the enemy while idle.                
    public float visionConeAngle;                           //The horizontal FOV of the enemy.
    public float range;                                     //The range at which the enemy starts chasing the player.

    [Header("Position")]
    public float approachRange;                             //The range at which the enemy starts approaching the player.
    public float backoffRange;                              //The range at which the enemy starts backing off from the player.
    public float movementSpeed;                             //The movement speed of the enemy.

    [Header("Attack")]
    public AudioSource audioSource;
    public Animator attackSpriteAnimator;
    public AnimationClip attackSpriteAnimation;
    public AudioClip attackAudio;
    public GameObject eye;                                  //The enemy's eye.
    public GameObject projectile;                           //The projectile the enemy spawns.
    public float attackLength;                              //Delay->Attack->Delay, the delays are 1/2 of the attack length.

    private StateMachine stateMachine;                      //The state machine responsible for maintaining the different states/behaviours of the enemy.
    private BasicMeleeIdleState idleState;                  //The idle behaviour of the enemy. Ticks only when the player is out of range/not seen by the enemy.
    private BasicRangedPosition positionState;              //The positioning behaviour of the enemy, ticks only when the enemy is too close or far from the player.
    private BasicRangedAttackState attackState;             //The attack behaviour of the enemy, ticks only when the enemy is in between approachRange and backoffRange.
    private Rigidbody RB;                                   //The rigid body of the enemy, used to move the enemy.


    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();

        //Converting the given angle from degrees to radians and calculating the cosine of that as the dot product produces the cos(angle in radians).
        visionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);

        //Starts by setting up the state machine and needed states along with other variables.
        RB = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        idleState = new BasicMeleeIdleState(gameObject, visionConeAngle, range, idleFloatingCurve);
        positionState = new BasicRangedPosition(gameObject, movementSpeed, approachRange, backoffRange, RB);
        attackState = new BasicRangedAttackState(gameObject, attackLength, eye, projectile, attackSpriteAnimator, attackSpriteAnimation, audioSource, attackAudio, this);

        //Setting up the transitions
        AT(idleState, positionState, InPositionRange());
        AT(positionState, idleState, ReturnToIdle());
        AT(positionState, attackState, InAttackRange());
        AT(attackState, positionState, FinishedAttack());
        AT(idleState, attackState, PlayerDetected());

        //Assigning the initial state of the state machine.
        stateMachine.SetState(idleState);


        //A wrapper function to make the transition setup cleaner.
        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        //The conditions for each state transition.
        Func<bool> InPositionRange() => () => (idleState.playerDistance < backoffRange || idleState.playerDistance > approachRange) && idleState.sawPlayer;
        Func<bool> ReturnToIdle() => () => positionState.hDistanceFromPlayer > range;
        Func<bool> InAttackRange() => () => positionState.inAttackRange;
        Func<bool> FinishedAttack() => () => attackState.attackDone;
        Func<bool> PlayerDetected() => () => idleState.sawPlayer && (idleState.playerDistance > backoffRange && idleState.playerDistance < approachRange);

    }

    // Update is called once per frame
    //Tick the currentState and ONLY the current state. Other states don't tick unless they're pickedUp.
    public void Update() => stateMachine.Tick();


}
