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
    int keyPressed;


    public int currentWeapon;
    int previousWeapon;


    // Start is called before the first frame update
    void Start()
    {
        if (!(hasWeapon[0] || hasWeapon[1] || hasWeapon[2]))
            currentWeapon = -1;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            currentWeapon = (int)WeaponCodes.katana;
            hasWeapon[(int)WeaponCodes.katana] = true;
        }

        if (!weapons[(int)WeaponCodes.katana])
            weapons[(int)WeaponCodes.katana] = GameObject.FindGameObjectWithTag("PlayerKatana");
        if (!weapons[(int)WeaponCodes.revolver])
            weapons[(int)WeaponCodes.revolver] = GameObject.FindGameObjectWithTag("PlayerRevolver");
        if (!weapons[(int)WeaponCodes.shotgun])
            weapons[(int)WeaponCodes.shotgun] = GameObject.FindGameObjectWithTag("PlayerShotgun");

        previousWeapon = currentWeapon;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i != currentWeapon)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                weapons[i].SetActive(true);
                currentWeaponScript = weapons[i].GetComponent<BaseWeaponScript2>();
            }
        }
        if(currentWeaponScript)
            currentWeaponScript.WeaponState_Script = (int)WeaponState.idle;

        AmmoText = GameObject.Find("AmmoText");
        if (AmmoText && currentWeapon < 2)
            AmmoText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWeapons();
        ShootWeapon();

    }

    void SwitchWeapons()
    {
        keyPressed = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                print("Katana");
                keyPressed = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                print("Revolver");
                keyPressed = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                print("Shotgun");
                keyPressed = 2;
            }
            if (keyPressed != -1)
            {
                previousWeapon = currentWeapon;
                currentWeapon = keyPressed;

                weapons[previousWeapon].SetActive(false);
                weapons[currentWeapon].SetActive(true);
                currentWeaponScript = weapons[currentWeapon].GetComponent<BaseWeaponScript2>();
                currentWeaponScript.WeaponState_Script = (int)WeaponState.idle;

                if (currentWeapon == (int)WeaponCodes.katana)
                    AmmoText.SetActive(false);
            }
        }
    }
    void ShootWeapon()
    {
        if (currentWeaponScript && Input.GetKeyDown(KeyCode.Mouse0))
            currentWeaponScript.Shoot();
    }

}
