using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System;


public class KatanaWeaponPickup : BasePickup
{
    public AnimationClip katanaPickupAnim;
    public AnimationCurve DOFStrength;

    WeaponSystemScript weaponSys;

    float timeSincePickup = 0;
    Animator animator;

    Volume ppVol;
    public override void InitiatePickup()
    {
        StartCoroutine(delayedAction(() => { DisableShootingAndMovement(); }, 2));
        weaponSys = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSystemScript>();
        if (weaponSys)
        {
            // Give the waepon to the player and set it as active
            weaponSys.switchedWeapons = true;
            weaponSys.currentWeapon = (int)WeaponCodes.katana;
            weaponSys.hasWeapon[(int)WeaponCodes.katana] = true;
            weaponSys.weapons[(int)WeaponCodes.katana].SetActive(true);
        }
        animator = weaponSys.weapons[weaponSys.currentWeapon].GetComponent<Animator>();
        if (animator)
        {
            ppVol.enabled = true;
            animator.enabled = true;
            animator.Play(katanaPickupAnim.name);
            Invoke(nameof(EnableShootingAndMovement), katanaPickupAnim.length);
        }
    }

    void DisableShootingAndMovement()
    {
        PlayerVariables.playerRB.isKinematic = true;
        weaponSys.currentWeaponScript.canShoot = false;
        PlayerVariables.walkScript.enabled = false;
    }

    void EnableShootingAndMovement()
    {
        PlayerVariables.playerRB.isKinematic = false;
        PlayerVariables.walkScript.enabled = true;
        weaponSys.currentWeaponScript.canShoot = true;
        ppVol.enabled = false;

        Destroy(gameObject);
    }
    IEnumerator delayedAction(UnityAction action, int nFrames)
    {
        yield return null;
        for (int i = 0; i < nFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        action.Invoke();
    }

    private void Start()
    {
        //print(Camera.main.GetComponents<Volume>().Length);
        ppVol = Camera.main.GetComponents<Volume>()[1];
    }

    private void Update()
    {
        if (!ppVol || !ppVol.enabled)
            return;

        ppVol.weight = DOFStrength.Evaluate(timeSincePickup / katanaPickupAnim.length);

        timeSincePickup += Time.deltaTime;
    }
}
