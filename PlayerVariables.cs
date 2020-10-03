using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A container responsible for storing commonly used / shared variables.
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerVariables", order = 1)]
public class PlayerVariables : ScriptableObject
{
    public PlayerHealthManager pHM;
    public CapsuleCollider playerCapsule;       //The player's collision capsule, used for groundchecking among other things.
    public Rigidbody playerRB;                  //The player's rigid body, used to apply forces and velocities.
    public GameObject player;                   //The player's game object, used to get position and rotation.
    public GameObject playerCam;                //The player's camera, used to rotate it and get it's positions and vectors.
    public GameObject playerCamParent;          //The player's camera parent, used to tilt it while wallrunning. (No, rotating the camera itself doesn't work)
    public Vector3 wallrunNormal;               //The normal vector where the wallrun check hit.
    public float maxVelocity = 40f;             //The maximum velocity a player can walk/run.
    public bool[] isGrounded = { true, true };  //Stores whether the player is on the ground or not.
    public bool isCrouching = false;            //Is the player crouching?
    public bool isDashing = false;              //Is the player dashing?    
    public bool isWallrunning = false;          //Is the player wallrunning?
    public bool isClimbing = false;             //Is the player climbing?
    public bool isGrappling = false;            //Is the player grappling?
    public float x, y;                          //What WASD key is pressed.
    public bool jumping;                        //Did the player tap the spacebar?
    public bool spaceHeld = false;              //Is the player holding down the spacebar?
    public float secondsSinceLevelStart;        //What's the time since the player spawned?


    public Vector3 playerPos => player.transform.position;
    public CapsuleCollider playerCap => player.GetComponent<CapsuleCollider>();
    public float playerHealth => pHM.currentHealth;
    public float playerMaxHealth => pHM.maxHealth;
    public Vector3 inFrontOfCam => playerCam.transform.position + playerCam.transform.forward;
    public Vector3 cameraPos => playerCam.transform.position;
    public Vector3 cameraFwd => playerCam.transform.forward;

}