using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for providing visual feedback and raycasting to damage the player.
public class BasicMeleeAttackState : StateInterface
{
    public bool attackFinished = false;             //Whether or not the attack has finished.

    ParticleSystem attackPS;                        //The particle system used to provide feedback.
    PlayerVariables pVars;                          //A container responsible for storing commonly used / shared variables. Passed on from the main script.
    GameObject entity;                              //A reference to the enemy.
    GameObject smile;                               //A reference to the smile.
    GameObject poker;                               //A reference to the poker face.
    Rigidbody RB;                                   //A reference to the rigid body used to move the enemy.

    RaycastHit[] hit;                               //Stores the result of the raycast during the attack rotation.
    Vector3 enemyPlayerVec;                         //The normalized vector originating at the enemy and ending at the player. Used for moving the enemy and rotating it. 

    float timer = 0;                                //A timer used to keep track of the attack progress.
    float damage = 10f;
    float beforeAttackDelay = 0;                    //The initial delay before the attack rotation starts.
    float rotationDuration = 0.25f;                 //The length of the rotation animation.
    float rotationEnd;                              //When the rotation should end relative to the attack start time.
    float particleDelay = 0.175f;                   //How long after finishing the rotation should the particle effect be visible for?
    float afterAttackDelay = 1f;                    //The delay after the attack before re-entering the state again or switching to another state.
    float attackLength;                             //How long in total the attack lasts for.

    //Attack start------------attack delay--------------rotation end----------attack length.
    //------------[Phase 1    ]----------[Phase 2       ]----------[Phase 3                ]
    //Phase 1, the enemy stops to give the player a chance to react.
    //Phase 2, rotation to give the player visual feedback.
    //Phase 3, Some delay before the next attack.
    //Phase 2.5, does a raycast to detect if the player is hit, here we should damage the player if the raycast hit.

    public BasicMeleeAttackState(float inputDamage, float inputBeforeAttackDelay, float inputRotationDuration, float inputParticleDelay, float inputAfterAttackDelay,
                                    GameObject inputEntity, Rigidbody inputRB, GameObject inputSmile, GameObject inputPoker, ParticleSystem inputAttackPS, PlayerVariables inputPVars)
    {
        damage = inputDamage;
        beforeAttackDelay = inputBeforeAttackDelay;
        entity = inputEntity;
        RB = inputRB;
        smile = inputSmile;
        poker = inputPoker;
        attackPS = inputAttackPS;
        pVars = inputPVars;

    }

    public void Tick()
    {
        //Tick-Tock
        timer += Time.deltaTime;

        //If the enemy is in phase 2 (rotation), plays the particle effect and in the middle of rotation does a ray cast to damage the player.
        if (timer > beforeAttackDelay && timer < rotationEnd)
        {
            //Playing the particle effect.
            attackPS.gameObject.SetActive(true);
            attackPS.Play();

            //If the enemy is in the middle of phase 2 (hitting), does the ray cast.
            if (timer <= rotationEnd - 0.5 * rotationDuration + 1 * Time.deltaTime && timer >= rotationEnd - 0.5 * rotationDuration)
            {
                //Debug.Log("Raycast");
                hit = Physics.RaycastAll((entity.transform.position - entity.transform.forward)/*+ Vector3.up * 2*/, -(entity.transform.forward), 4f);
                Debug.DrawRay((entity.transform.position - entity.transform.forward), -(entity.transform.forward * 4), Color.red, 10f);
                if (hit.Length > 0 && hit[0].collider.CompareTag("Player"))
                {
                    Debug.Log("Player hit"); //Player damage code here.

                    PlayerHealthManager playerHM = hit[0].collider.GetComponent<PlayerHealthManager>();
                    if (playerHM != null)
                        playerHM.RecieveDamage(damage);
                }
            }

            //Rotates the enemy
            entity.transform.rotation = Quaternion.AngleAxis(360 / rotationDuration * Time.deltaTime, Vector3.up) * entity.transform.rotation;
        }
        //If the enemy is in phase 1 or phase 3 (delays), stops the enemy in place to give the player a chance to dodge out of the way.
        else if (timer < beforeAttackDelay || timer > rotationEnd && timer < attackLength)
        {
            //After the rotation end, after the particle delay, stops the particle effect.
            //In the first age, in the first battle, when the shadows first lengthened, one stood. Burned by the embers of Armageddon,
            //his soul blistered by the fires of Hell and tainted beyond ascension, he chose the path of perpetual torment. In his ravenous hatred he found no peace;
            //and with boiling blood he scoured the Umbral Plains seeking vengeance against the dark lords who had wronged him. He wore the crown of the Night Sentinels,
            //and those that tasted the bite of his sword named him... the Doom Slayer.
            if (timer > rotationEnd + particleDelay)
            {
                attackPS.gameObject.SetActive(false);
                attackPS.Stop();
            }

            //Stopping the enemy in place.
            RB.velocity = Vector3.zero;
            enemyPlayerVec = pVars.player.transform.position - entity.transform.position;
            enemyPlayerVec.y = 0;
            enemyPlayerVec.Normalize();
            entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, Quaternion.LookRotation(enemyPlayerVec, Vector3.up), Time.deltaTime * 2000f);
        }
        //if the enemy finished the attackm exits the state.
        else if (timer > attackLength)
            attackFinished = true;
    }

    public void OnEnter()
    {
        //Switches the smile to a pokerface.
        if (smile)
            smile.SetActive(false);
        if (poker)
            poker.SetActive(true);

        //Calculates the times.
        rotationEnd = beforeAttackDelay + rotationDuration;
        attackLength = rotationEnd + afterAttackDelay;
    }

    public void OnExit()
    {
        //Cleanup.
        if (smile)
            smile.SetActive(true);
        if (poker)
            poker.SetActive(false);
        attackFinished = false;
        hit = null;
        timer = 0;
    }
}
