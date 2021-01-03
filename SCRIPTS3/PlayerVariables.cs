using System.Collections;
using UnityEngine;
using TMPro;

public enum Key { none, red, green, blue, yellow };

//A container responsible for storing commonly used / shared variables.
public static class PlayerVariables
{
    public static float timeSinceUngrounded;

    public static bool paused;
    public static Animator KatanaAnimator;
    public static Key HeldKey;

    //UI Text
    public static Animator UITextAnimator;
    public static TextMeshProUGUI UITextScript;
    public static AnimationClip UITextAnimation;


    public static WalkScript walkScript;
    public static PlayerHealthManager pHM;
    public static CapsuleCollider playerCapsule;       //The player's collision capsule, used for groundchecking among other things.
    public static Rigidbody playerRB;                  //The player's rigid body, used to apply forces and velocities.
    public static GameObject player;                   //The player's game object, used to get position and rotation.
    public static GameObject playerCam;                //The player's camera, used to rotate it and get it's positions and vectors.
    public static GameObject playerCamParent;          //The player's camera parent, used to tilt it while wallrunning. (No, rotating the camera itself doesn't work)
    public static Vector3 wallrunNormal;               //The normal vector where the wallrun check hit.
    public static float maxVelocity = 40f;             //The maximum velocity a player can walk/run.
    public static bool prevFrameGrounded = true;
    public static bool isGrounded;                     //Stores whether the player is on the ground or not.
    public static bool isCrouching = false;            //Is the player crouching?
    public static bool isDashing = false;              //Is the player dashing?    
    public static bool isWallrunning = false;          //Is the player wallrunning?
    public static bool isClimbing = false;             //Is the player climbing?
    public static bool isGrappling = false;            //Is the player grappling?
    public static float x, y;                          //What WASD key is pressed.
    public static bool pressedJump;                    //Did the player tap the spacebar?
    public static bool spaceHeld = false;              //Is the player holding down the spacebar?
    public static float secondsSinceLevelStart;        //What's the time since the player spawned?

    public static Vector3 playerPos => player.transform.position;
    public static CapsuleCollider playerCap => player.GetComponent<CapsuleCollider>();
    public static float playerHealth => pHM.currentHealth;
    public static float playerMaxHealth => pHM.maxHealth;
    public static Vector3 inFrontOfCam => playerCam.transform.position + playerCam.transform.forward;
    public static Vector3 cameraPos => playerCam.transform.position;
    public static Vector3 cameraFwd => playerCam.transform.forward;

    public static IEnumerator ShowText(string text, float duration)
    {
        UITextScript.gameObject.SetActive(true);
        UITextScript.text = text;
        UITextAnimator.Play(UITextAnimation.name + "Fwd");

        yield return new WaitForSeconds(UITextAnimation.length + duration);
        //print("b");

        UITextAnimator.Play(UITextAnimation.name + "Bck");

        yield return new WaitForSeconds(UITextAnimation.length);

        UITextScript.gameObject.SetActive(false);
    }

    public static string GetKeyName(Key key)
    {
        if (key == Key.none)
            return "No";
        else if (key == Key.red)
            return "Red";
        else if (key == Key.green)
            return "Green";
        else if (key == Key.blue)
            return "Blue";
        else if (key == Key.yellow)
            return "Yellow";
        else
            return " ";
    }
}