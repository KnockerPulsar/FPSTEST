using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
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
        StartCoroutine(delayedAction(() => { DisableShooting(); }, 2));
        weaponSys = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSystemScript>();
        if (weaponSys)
        {
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
            Invoke(nameof(EnableShooting), katanaPickupAnim.length);
        }
    }

    void DisableShooting()
    {
        print("D");
        weaponSys.currentWeaponScript.canShoot = false;
    }

    void EnableShooting()
    {
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
        print(Camera.main.GetComponents<Volume>().Length);
        ppVol = Camera.main.GetComponents<Volume>()[1];
    }

    private void Update()
    {
        if (!ppVol || !ppVol.enabled)
            return;

        ppVol.weight = DOFStrength.Evaluate(timeSincePickup);

        timeSincePickup += Time.deltaTime;
    }
}
