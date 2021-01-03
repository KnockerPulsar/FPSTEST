using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum WeaponState { idle, shooting, reloading, grappling }

public class BaseWeaponScript : MonoBehaviour
{
    public int currentWeaponState = 0;
    public GameObject projectile;
    public GameObject muzzle;
    public GameObject ammoTextMeshPro;
    public TextMeshProUGUI ammoText;

    public bool canShoot = true;
    public bool isReloading = false;

    //Base stats
    public float damage = 100f;
    public float fireRate = 0.5f;
    public float clipSize = 6f;
    public float currentAmmoInClip = 6f;
    public float reloadTime = 1f;
    public float projectileSpeed = 200f;
    public float range = 10f;


    //Multipliers
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float clipSizeMultiplier = 1f;
    public float reloadTimeMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public float rangeMultiplier = 1f;



    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
     
    }

    public virtual void Shoot()
    {

    }

    public virtual void Reload()
    {

    }

    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {

    }

}
