using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Responsible for spawning a projectile when the enemy is in the appropriate range.
public class BasicRangedAttackState : StateInterface
{
    public bool attackDone;                 //A flag to indicate whether the attack is done or not.

    PlayerVariables pVars;                  //A container responsible for storing commonly used / shared variables.
    GameObject entity;                      //A reference the enemy.
    GameObject enemyEye;                    //The enemy's eye.
    GameObject projectile;                  //The projectile the enemy spawns.
    ParticleSystem attackPS;                //The particle system shown when the enemy attacks.
    float timer;                            //An internal timer.
    float attackLength;                     //Delay->Attack->Delay, the delays are 1/2 of the attack length.
    Vector3 enemyPlayerVec;                 //A vector originating at the enemy and pointing towards the player.

    public BasicRangedAttackState(GameObject inputEntity, float inputAL, GameObject inputEnemyEye, GameObject inputProjectile, ParticleSystem inputPS, PlayerVariables inputPVars)
    {
        entity = inputEntity;
        attackLength = inputAL;
        enemyEye = inputEnemyEye;
        projectile = inputProjectile;
        attackPS = inputPS;
        pVars = inputPVars;
    }

    public void Tick()
    {
        //Tick tock, tick tock...
        timer += Time.deltaTime;

        //Rotating the enemy towards the player
        enemyPlayerVec = pVars.player.transform.position - entity.transform.position;
        enemyPlayerVec.Normalize();
        entity.transform.forward = Vector3.Slerp(entity.transform.forward, enemyPlayerVec, Time.deltaTime * 2000f);

        //Spawning the projectile at the middle (± a frame or so) of the attack.
        if (timer < attackLength / 2f + Time.deltaTime && timer > attackLength / 2f)
        {
            attackPS.gameObject.SetActive(true);
            attackPS.Play();

            GameObject.Instantiate(projectile, enemyEye.transform.position + enemyEye.transform.up, Quaternion.LookRotation(entity.transform.up, entity.transform.forward));
        }

        //When the attack is done (after the second delay), raises the flag.
        else if (timer > attackLength)
            attackDone = true;
    }

    public void OnEnter()
    {
        attackDone = false;
        timer = 0;
    }

    public void OnExit()
    {
        attackPS.gameObject.SetActive(false);
        attackPS.Stop();
    }
}
