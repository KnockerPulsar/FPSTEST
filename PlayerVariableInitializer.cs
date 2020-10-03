﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for initializing the pVars variables and maintaining them.
public class PlayerVariableInitializer : MonoBehaviour
{
    public PlayerVariables pVars;           //A container responsible for storing commonly used / shared variables.

    //Caches commonly used/ shares variables at the start of the game / level.
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        pVars.playerCapsule = GetComponent<CapsuleCollider>();
        pVars.playerRB = GetComponent<Rigidbody>();
        pVars.player = GameObject.FindGameObjectWithTag("Player");
        if (!Camera.main.gameObject)
            print("A");
        pVars.playerCam = Camera.main.gameObject;
        pVars.playerCamParent = GameObject.Find("Cameras");
        pVars.isCrouching = false;
        pVars.isDashing = false;
        pVars.secondsSinceLevelStart = 0;
        pVars.pHM = GetComponent<PlayerHealthManager>();
    }


    //Checks if the player on the ground, if the spacebar is pressed or held, if any of the WASD keys is pressed and maintains the time since spawn.
    private void Update()
    {
        pVars.isGrounded[0] = pVars.isGrounded[1];
        pVars.isGrounded[1] = Physics.Raycast(transform.position, transform.up * -1f, pVars.playerCapsule.height / 1.7f);
        pVars.jumping = Input.GetKeyDown(KeyCode.Space);
        pVars.spaceHeld = Input.GetKey(KeyCode.Space);
        pVars.x = Input.GetAxisRaw("Horizontal");
        pVars.y = Input.GetAxisRaw("Vertical");
        pVars.secondsSinceLevelStart += Time.deltaTime;
    }

}
