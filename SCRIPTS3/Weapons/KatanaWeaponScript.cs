using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class KatanaWeaponScript : BaseWeaponScript
{
     
    public GameObject hitMarker;
    public AnimationClip[] AttackAnims;
    public AudioClip[] HitSFX;
    public AudioSource hitAudio;
    public Vector3 attackHitBox = Vector3.one;
    public float hitPauseLength = 0.1f;

    Vector3 hitBoxCenter;
    Quaternion hitBoxRot;

    int rand;
    Camera playerCam;
    Vector3 dashFinal;
    Animator katanaAnimator;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerCam = Camera.main;
        katanaAnimator = GetComponent<Animator>();
    }

    public override void Shoot()
    {
        if (currentWeaponState == (int)WeaponState.idle && canShoot)
        {
            base.currentWeaponState = (int)WeaponState.shooting;

            rand = Random.Range(0, AttackAnims.Length);
            katanaAnimator.Play(AttackAnims[rand].name);
            Invoke(nameof(EndShoot), katanaAnimator.GetCurrentAnimatorClipInfo(0).Length / 2f);
        }
    }

    void SliceEvent()
    {
        hitBoxCenter = PlayerVariables.playerPos + PlayerVariables.playerCam.transform.forward * attackHitBox.z;
        Collider[] hitCols = Physics.OverlapBox(hitBoxCenter, attackHitBox, Quaternion.LookRotation(playerCam.transform.forward));

        foreach (Collider hit in hitCols)
        {
            Transform checkObj;
            if (hit.transform.parent)
                checkObj = hit.transform.parent;
            else
                checkObj = hit.transform;

            if (checkObj.CompareTag("Enemy"))
            {
                EnemyHealth EH = checkObj.GetComponentInParent<EnemyHealth>();
                if (EH != null)
                {
                    StartCoroutine(HitPause());
                    hitMarker.GetComponent<Animator>().SetTrigger("Show");
                    rand = Random.Range(0, HitSFX.Length);
                    hitAudio.clip = HitSFX[rand];
                    hitAudio.Play();
                    EH.RecieveDamage(damage * damageMultiplier);
                }
                else
                    print("!EH");
            }
        }
    }

    void EndShoot()
    {
        base.currentWeaponState = (int)WeaponState.idle;
        //katanaAnimator.SetInteger("WeaponState_Anim", currentWeaponState);
    }



    private void OnDrawGizmos()
    {
        if (currentWeaponState == (int)WeaponState.shooting)
        {
            //Debug
            Gizmos.matrix = Matrix4x4.TRS(hitBoxCenter, Quaternion.LookRotation(playerCam.transform.forward), attackHitBox);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }

    IEnumerator HitPause()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(hitPauseLength);

        Time.timeScale = 1f;
    }

}

