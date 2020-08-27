using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for handling the jumping ability of the player.
public class JumpScript : MonoBehaviour
{
    public PlayerVariables pVars;           //A container responsible for storing commonly used / shared variables.
    public float jumpForce = 30f;           //Used to determine the jump force and thus it's height.


    // Update is called once per frame
    //Checks if the player pressed the space bar (pVars.jumping) and if they're on the ground and if the player is not Wallrunning.
    //If so, initiates a jump.
    void Update()
    {
        if (pVars.jumping && pVars.isGrounded[1] && !pVars.isWallrunning)
            Jump();
    }

    //The jump is dependant on the multiplier and the player's mass and the max velocity the player can walk/run.
    void Jump()
    {
        pVars.playerRB.AddForce(Vector3.up * jumpForce * pVars.playerRB.mass * pVars.maxVelocity);
        //print("yikes");
    }
}
