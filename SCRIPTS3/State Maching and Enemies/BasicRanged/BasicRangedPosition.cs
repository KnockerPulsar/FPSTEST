using System.Collections;
using System.Collections.Generic;

using UnityEngine;


//Coroutines can be started in non MonoBehaviour classes by passing a reference to a MonoBehaviour based class and starting the coroutine through that.
public class BasicRangedPosition : IState
{
    public bool inAttackRange;
    public float hDistanceFromPlayer;

    GameObject entity;                          //A reference to the enemy.
    Rigidbody RB;                               //The rigid body component used to move the enemy.

    float movementSpeed;                        //The movement speed of the enemy.
    float approachRange;                        //The range at which the enemy starts approaching the player.
    float backoffRange;                         //The range at which the enemy starts backing off from the player.

    float dir;                                  //A variable used to determine whether to move towards or away from the player.
    Vector3 hPlayerPos;                         //The horizontal position of the player.
    Vector3 hEnemyPlayerVec;                    //A vector pointing towards the player from the enemy, has the y component modified so that both the enemy and the player are in the same plane.
    Vector3 enemyPlayerVec;                     //A vector pointing towards the player from the enemy.


    //BackoffRange < ApproachRange < Range

    public BasicRangedPosition(GameObject inputEntity, float inputMS, float inputAR, float inputBFR, Rigidbody inputRB)
    {
        entity = inputEntity;
        movementSpeed = inputMS;
        approachRange = inputAR;
        backoffRange = inputBFR;
        RB = inputRB;
    }

    public void Tick()
    {
        //Finding the enemy-player vector and rotating the enemy so it's forward vector is in that direction.
        enemyPlayerVec = PlayerVariables.player.transform.position - entity.transform.position;
        enemyPlayerVec.Normalize();
        entity.transform.forward = Vector3.Slerp(entity.transform.forward, enemyPlayerVec, Time.deltaTime * 2000f);

        //Finding the horizontal vector between the player and the enemy, then calculating the horizontal distance between them.
        hPlayerPos = PlayerVariables.player.transform.position;
        hPlayerPos.y = entity.transform.position.y;

        hDistanceFromPlayer = Vector3.Distance(entity.transform.position, hPlayerPos);

        //If the enemy is too close or too far from the player, moves the enemy farther or closer.
        if (hDistanceFromPlayer > approachRange || hDistanceFromPlayer < backoffRange)
        {
            hEnemyPlayerVec = hPlayerPos - entity.transform.position;
            hEnemyPlayerVec.Normalize();

            if (hDistanceFromPlayer > approachRange)
                //Come closer to the player.
                dir = 1;

            else if (hDistanceFromPlayer < backoffRange)
                //Back off from the player.
                dir = -1;

            RB.AddForce(dir * hEnemyPlayerVec * movementSpeed * Time.deltaTime);
        }
        //The moment the enemy gets into proper range, does a dash either forward, left or right.
        else
        {
            //Debug.Log("Side");
            //Go to the left or right randomly 
            dir = Mathf.Round(Random.Range(-1, 2));
            if (dir != 0)
                RB.AddForce(entity.transform.right * dir * movementSpeed / 2f);
            else
                RB.AddForce(entity.transform.forward * movementSpeed / 2f);
            inAttackRange = true;
        }

    }

    public void OnEnter() { inAttackRange = false; }

    public void OnExit() { RB.velocity = Vector3.zero; }
}
