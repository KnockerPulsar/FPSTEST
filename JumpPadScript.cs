using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//Responsible for activating the jump pads whenever a player enters the collision box
//Bounces the player in the direcetion normal to the upper face (the pad's local Y axis).
public class JumpPadScript : MonoBehaviour
{
    public float jumpForceMultiplier = 5f;          //Used to determine the magnitude of the bounce.

    //If the player collides with the jump pad's hit box, preserves the horizontal components of their velocity 
    //(the x & z comps) and sets the vertical/Y component to the local Y axis * the multiplier. 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, 0, playerRigidBody.velocity.z) + transform.up * jumpForceMultiplier;
           
            //print("JUMP PAD");
        }
    }
}
