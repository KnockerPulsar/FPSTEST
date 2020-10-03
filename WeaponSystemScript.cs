using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum WeaponCodes { katana, revolver, shotgun }


//Responsible for managing the weapons the player has, shooting them and switching between them.
//Will probably be re-written soon, so documentation.
public class WeaponSystemScript : MonoBehaviour
{
    public GameObject[] weapons = { };
    public bool[] hasWeapon = { true, false, false };
    public BaseWeaponScript2 currentWeaponScript;
    GameObject AmmoText;
    public bool switchedWeapons;


    public int currentWeapon;
    int previousWeapon;


    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = -1;

        if (!(hasWeapon[0] || hasWeapon[1] || hasWeapon[2]))
            currentWeapon = -1;

        if (!weapons[(int)WeaponCodes.katana])
            weapons[(int)WeaponCodes.katana] = GameObject.FindGameObjectWithTag("PlayerKatana");
        if (!weapons[(int)WeaponCodes.revolver])
            weapons[(int)WeaponCodes.revolver] = GameObject.FindGameObjectWithTag("PlayerRevolver");
        if (!weapons[(int)WeaponCodes.shotgun])
            weapons[(int)WeaponCodes.shotgun] = GameObject.FindGameObjectWithTag("PlayerShotgun");

        previousWeapon = currentWeapon;

        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            if (i == currentWeapon)
            {
                weapons[i].SetActive(true);
                currentWeaponScript = weapons[i].GetComponent<BaseWeaponScript2>();
            }
            else
                weapons[i].SetActive(false);

            i++;
        }

        if (currentWeaponScript)
            currentWeaponScript.currentWeaponState = (int)WeaponState.idle;

        AmmoText = GameObject.Find("AmmoText");
        if (AmmoText && currentWeapon < 2)
            AmmoText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWeaponSwitch();

        if (switchedWeapons)
            SwitchWeapons();

        ShootWeapon();

    }

    void SwitchWeapons()
    {
        if(!hasWeapon[currentWeapon])
            return;

        switchedWeapons = false;

        if (previousWeapon >= 0 && previousWeapon < weapons.Length)
            weapons[previousWeapon].SetActive(false);
        weapons[currentWeapon].SetActive(true);
        currentWeaponScript = weapons[currentWeapon].GetComponent<BaseWeaponScript2>();
        currentWeaponScript.currentWeaponState = (int)WeaponState.idle;

        if (currentWeapon == (int)WeaponCodes.katana)
            AmmoText.SetActive(false);
    }
    void ShootWeapon()
    {
        if (currentWeaponScript && Input.GetKeyDown(KeyCode.Mouse0))
            currentWeaponScript.Shoot();
    }

    void CheckForWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            previousWeapon = currentWeapon;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                print("Katana");
                currentWeapon = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                print("Revolver");
                currentWeapon = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                print("Shotgun");
                currentWeapon = 2;
            }
            switchedWeapons = true;
        }
    }

}
