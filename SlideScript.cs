using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for the crouch/slide ability.
public class SlideScript : MonoBehaviour
{
    public PlayerVariables pVars;                       //A container responsible for storing commonly used / shared variables.
    public float slideDrag = 0.8f;                      //The rigidbody's drag when sliding.
    public float normalDrag = 1;                        //The rigidbody's drag when not sliding.
    public float slideVelocityMultiplier = 1.1f;        //The velocity multiplier used when initiating a slide.


    // Update is called once per frame
    //Checks if the player is pressing the crouch button or not and if the player isn't crouching or not
    //and changes the player's state depending on those.
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !pVars.isCrouching)
            Crouch();
        else if (!Input.GetKey(KeyCode.LeftControl) && pVars.isCrouching && CrouchCheck())
            EndCrouch();
    }

    //Updates the player's crouch boolean and changes the player's height and radius.
    //If the player's on the ground and is pressing the W key, adds some velocity to the player and changes the player's drag.
    private void Crouch()
    {
        pVars.isCrouching = true; //Changes the boolean to stop friction
        pVars.playerCapsule.height /= 2; //Halves the collision capsule's height
        pVars.playerCapsule.radius /= 2;
        if (pVars.isGrounded[1])
        {
            pVars.playerRB.drag = slideDrag; //Nullifies the drag of body
            pVars.playerRB.angularDrag = 0.05f;
            if (pVars.y > 0)
            {
                Vector3 slideDir = pVars.player.transform.forward;
                pVars.playerRB.velocity += (slideDir * pVars.y * pVars.maxVelocity * slideVelocityMultiplier);
            }
        }
    }

    //Updates the players crouch boolean, resets the height, radius and drags.
    private void EndCrouch()
    {
        //Does the opposite of the "Crouch" function
        pVars.isCrouching = false;
        pVars.playerCapsule.height *= 2f;
        pVars.playerCapsule.radius *= 2f;
        pVars.playerRB.drag = normalDrag;
        pVars.playerRB.angularDrag = 0.1f;
    }

    //Checks if the player is undersomething and if the player can stand up.
    bool CrouchCheck()
    {
        return !Physics.Raycast(transform.position, transform.up, pVars.playerCapsule.height * (3f));
    }

}
