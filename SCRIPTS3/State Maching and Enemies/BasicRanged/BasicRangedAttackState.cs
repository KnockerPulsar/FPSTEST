using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Responsible for spawning a projectile when the enemy is in the appropriate range.
public class BasicRangedAttackState : IState
{
    public bool attackDone;                 //A flag to indicate whether the attack is done or not.

    GameObject entity;                      //A reference the enemy.
    GameObject enemyEye;                    //The enemy's enemyEye.
    GameObject projectile;                  //The projectile the enemy spawns.
    Animator attackPS;
    AnimationClip attackPSAnim;
    //float timer;                            //An internal timer.
    float attackLength;                     //Delay->Attack->Delay, the delays are 1/2 of the attack length.
    Vector3 enemyPlayerVec;                 //A vector originating at the enemy and pointing towards the player.
    AudioSource audioSource;
    AudioClip attackAudio;

    BaseAIStateMachine SM;

    public BasicRangedAttackState(GameObject inputEntity, float inputAL, GameObject inputEnemyEye,
        GameObject inputProjectile, Animator attackPS, AnimationClip attackPSAnim, AudioSource inputAudioSource,
        AudioClip inputAttackAudio, BaseAIStateMachine stateMachine)
    {
        entity = inputEntity;
        attackLength = inputAL;
        enemyEye = inputEnemyEye;
        projectile = inputProjectile;
        this.attackPS = attackPS;
        this.attackPSAnim = attackPSAnim;

        audioSource = inputAudioSource;
        attackAudio = inputAttackAudio;

        SM = stateMachine;
    }

    public void Tick()
    {
        ////Tick tock, tick tock...
        //timer += Time.deltaTime;

        //Rotating the enemy towards the player
        enemyPlayerVec = PlayerVariables.player.transform.position - entity.transform.position;
        enemyPlayerVec.Normalize();
        entity.transform.forward = Vector3.Slerp(entity.transform.forward, enemyPlayerVec, Time.deltaTime * 2000f);

        ////Spawning the projectile at the middle (± a frame or so) of the attack.
        //if (timer < attackLength / 2f + Time.deltaTime && timer > attackLength / 2f)
        //{
        //    attackPS.gameObject.SetActive(true);
        //    attackPS.Play(attackPSAnim.name, 0);
        //    attackPS.Play(attackPSAnim.name, 0, -1);

        //    audioSource.PlayOneShot(attackAudio);

        //    GameObject.Instantiate(projectile, enemyEye.transform.position + enemyEye.transform.up, Quaternion.LookRotation(entity.transform.up, entity.transform.forward));
        //}

        ////When the attack is done (after the second delay), raises the flag.
        //else if (timer > attackLength)
        //    attackDone = true;
    }

    public void OnEnter()
    {
        attackDone = false;
        //timer = 0;
        SM.StartCoroutine(Attack());
    }

    public void OnExit()
    {
        attackPS.gameObject.SetActive(false);

    }

    //TODO: Make the sprite animation work properly.


    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackLength/2f);

        attackPS.gameObject.SetActive(true);
        attackPS.Play(attackPSAnim.name, 0);
        attackPS.Play(attackPSAnim.name, 0, -1);

        audioSource.PlayOneShot(attackAudio);

        GameObject.Instantiate(projectile, enemyEye.transform.position + enemyEye.transform.up, SM.transform.rotation);


        yield return new WaitForSeconds(attackLength / 2f);
        attackDone = true;
    }
}
