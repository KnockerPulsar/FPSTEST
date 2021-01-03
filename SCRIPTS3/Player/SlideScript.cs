using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for the crouch/slide ability.
public class SlideScript : MonoBehaviour
{
    //A container responsible for storing commonly used / shared variables.
    public float slideDrag = 0.8f;                      //The rigidbody's drag when sliding.
    public float normalDrag = 1;                        //The rigidbody's drag when not sliding.
    public float slideVelocityMultiplier = 1.1f;        //The velocity multiplier used when initiating a slide.


    private void Start()
    {
        PlayerVariables.isCrouching = false;
    }

    // Update is called once per frame
    //Checks if the player is pressing the crouch button or not and if the player isn't crouching or not
    //and changes the player's state depending on those.
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !PlayerVariables.isCrouching)
            Crouch();
        else if (!Input.GetKey(KeyCode.LeftControl) && PlayerVariables.isCrouching && CrouchCheck())
            EndCrouch();
    }

    //Updates the player's crouch boolean and changes the player's height and radius.
    //If the player's on the ground and is pressing the W key, adds some velocity to the player and changes the player's drag.
    private void Crouch()
    {
        PlayerVariables.isCrouching = true; //Changes the boolean to stop friction
        PlayerVariables.playerCapsule.height /= 2; //Halves the collision capsule's height
        PlayerVariables.playerCapsule.radius /= 2;
        if (PlayerVariables.isGrounded)
        {
            PlayerVariables.playerRB.drag = slideDrag; //Nullifies the drag of body
            PlayerVariables.playerRB.angularDrag = 0.05f;
            if (PlayerVariables.y > 0)
            {
                Vector3 slideDir = gameObject.transform.forward;
                PlayerVariables.playerRB.velocity += (slideDir * PlayerVariables.y * PlayerVariables.maxVelocity * slideVelocityMultiplier);
            }
        }
    }

    //Updates the players crouch boolean, resets the height, radius and drags.
    private void EndCrouch()
    {
        //Does the opposite of the "Crouch" function
        PlayerVariables.isCrouching = false;
        PlayerVariables.playerCapsule.height *= 2f;
        PlayerVariables.playerCapsule.radius *= 2f;
        PlayerVariables.playerRB.drag = normalDrag;
        PlayerVariables.playerRB.angularDrag = 0.1f;
    }

    //Checks if the player is undersomething and if the player can stand up.
    bool CrouchCheck()
    {
        return !Physics.Raycast(transform.position, transform.up, PlayerVariables.playerCapsule.height * (3f));
    }

}
