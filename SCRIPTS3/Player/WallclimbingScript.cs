using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallclimbingScript : MonoBehaviour
{
                    //A container responsible for storing commonly used / shared variables.
    public float wallClimbYVel = 40f;           //The Y velocity applied when climbing.
    RaycastHit[] inFront;                       //The climb raycast result.

    // Update is called once per frame
    //If the player is holding down the space bar, checks and initiates wallclimbing.
    void Update()
    {
        PlayerVariables.spaceHeld = Input.GetKey(KeyCode.Space);
        if (PlayerVariables.spaceHeld)
            Wallclimb();
    }

    //Checks if whatever's in front of the player is a climable object and if so, it updates the climbing boolean and sets the players vertical velocity to the wallClimbYVel variable's value.
    //It also zeros the forward velocity. 
    //If the player isn't holding down the space bar or there isn't any object in front or if the object isn't climable, stops climbing.
    void Wallclimb()
    {
        if (!PlayerVariables.isWallrunning)
        {
            inFront = Physics.RaycastAll(transform.position - new Vector3(0, PlayerVariables.playerCapsule.height, 0), transform.forward, 3f);
            Debug.DrawRay(transform.position - new Vector3(0, PlayerVariables.playerCapsule.height, 0), transform.forward * 3f, Color.red, 3f);


            //if (inFront.Length > 0 && inFront[0].collider.CompareTag("Climbable"))
            foreach (RaycastHit hit in inFront)
                if (hit.collider.CompareTag("Climbable"))
                {
                    if (!PlayerVariables.isClimbing)
                        PlayerVariables.isClimbing = true;

                    //Debug.Log("Checking for climbables");

                    //Adds some upwards velocity to mimic climbing
                    //Could be done by setting the velocity for more consistent velocity
                    //Debug.Log("Found something climbable");
                    //Zeros the forward velocity to prevent getting stuck
                    PlayerVariables.playerRB.velocity = new Vector3(0, wallClimbYVel, PlayerVariables.playerRB.velocity.z);

                }
            if (!PlayerVariables.spaceHeld || inFront == null || inFront.Length == 0 || !inFront[0].collider.CompareTag("Climbable"))
                PlayerVariables.isClimbing = false;
        }
    }
}
