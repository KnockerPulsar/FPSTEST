using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for providing visual feedback and raycasting to damage the player.
public class BasicMeleeAttackState : IState
{
    public bool attackFinished = false;             //Whether or not the attack has finished.

    float damage = 10f;
    float beforeAttackDelay = 0;                    //The initial delay before the attack rotation starts.
    float afterAttackDelay = 1f;                    //The delay after the attack before re-entering the state again or switching to another state.
    BaseAIStateMachine SM;
    AnimationClip attackAnimation;
    Animator animator;


    public BasicMeleeAttackState(float inputDamage, float inputBeforeAttackDelay, float inputAfterAttackDelay, Animator inputAnimator , AnimationClip inputAttackAnim, BaseAIStateMachine inputSM)
    {
        damage = inputDamage;
        beforeAttackDelay = inputBeforeAttackDelay;
        afterAttackDelay = inputAfterAttackDelay;
        animator = inputAnimator;
        attackAnimation = inputAttackAnim;
        SM = inputSM;
    }

    public void Tick() { /*Debug.Log(attackFinished);*/ }

    public void OnEnter()
    {
        animator.enabled = true;
        animator.Play(attackAnimation.name);
    }

    public void OnExit()
    {
        attackFinished = false;
    }

    public IEnumerator Attack()
    {
        Collider[] hits = Physics.OverlapBox(SM.transform.position, Vector3.one / 2f , Quaternion.identity);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealthManager playerHM = hit.GetComponent<PlayerHealthManager>();
                if (playerHM != null)
                    playerHM.RecieveDamage(damage);
                break;
            }
        }

        yield return new WaitForSeconds(afterAttackDelay);
        attackFinished = true;

        //yield return new WaitForSeconds(attackAnimation.length);
        DisableAnimator();
    }

    void DisableAnimator()
    {
        animator.enabled = false;
    }
}
