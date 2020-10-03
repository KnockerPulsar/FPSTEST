using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for the walking/running of the player.
public class WalkScript : MonoBehaviour
{
    public PlayerVariables pVars;                           //A container responsible for storing commonly used / shared variables.
    public float moveForce = 2500f;                         //The walk force multiplier.
    public float countermovementMultiplier = 0.5f;          //Friction multiplier.
    Vector2 horizontalVel;                                  //The Horizontal velocity of the player relative to their forward vector.
    RaycastHit[] movementCheckHits;                         //The results of the movement check raycast.

    //FindVelRealtiveToLook
    float lookAngle;                                        //The y rotation of the player
    float moveAngle;                                        //The y rotation of the player's velocity.
    float u;                                                //The angle difference between the look and move angle.
    float magnitude;                                        //The magnitude of the player's velocity.
    float yMag;                                             //The z/forward component of the velocty (Y component from top down).
    float xMag;                                             //The x/right componend of the velocity (X component from top down).
    float multiplier = 1f;

    //Using FixedUpdate for consistent movement.
    //If the player isn't on the ground, checks if the player is moving into a wall and if so, stops movement
    //This is to prevent players from latching onto unwanted walls.
    void FixedUpdate()
    {
        Move();
    }


    //Fires a raycast based on the WASD keys the player is pressing, returns true if the player has no object in that direction or there's a climbabl/latchable object.

    //Finds the velocity relative to the player's forward direction (duh).
    private Vector2 FindVelRelativeToLook()
    {
        lookAngle = transform.eulerAngles.y;
        moveAngle = Mathf.Atan2(pVars.playerRB.velocity.x, pVars.playerRB.velocity.z) * Mathf.Rad2Deg;

        u = Mathf.DeltaAngle(lookAngle, moveAngle);
        magnitude = pVars.playerRB.velocity.magnitude;
        yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        xMag = magnitude * Mathf.Sin(u * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    //Applies countermovement/friction to the player only when the player isn't pressing W/S or A/D.
    private void CounterMovement(float x, float y, Vector2 Mag)
    {
        //Should only apply while on the ground
        if (!pVars.isGrounded[1]) return;
        else if (pVars.isCrouching)
            countermovementMultiplier = 0.1f;
        else
            countermovementMultiplier = 0.5f;

        if (pVars.isGrounded[1] || pVars.isDashing)
        {
            //Only applies the force when the player isn't inputting movement in any of the axis, only applies to that axis
            if (x == 0 && Mag.x != 0)
                pVars.playerRB.AddForce(moveForce * pVars.playerRB.mass * transform.right * -Mag.x * countermovementMultiplier);
            if (y == 0 && Mag.y != 0)
                pVars.playerRB.AddForce(moveForce * pVars.playerRB.mass * transform.forward * -Mag.y * countermovementMultiplier);
        }

    }

    //Applies the movement and countermovement.
    void Move()
    {
        horizontalVel = FindVelRelativeToLook();

        if (!pVars.isCrouching)
            CounterMovement(pVars.x, pVars.y, horizontalVel);

        if (pVars.y != 0 && horizontalVel.y < pVars.maxVelocity)
            pVars.playerRB.AddForce(transform.forward * pVars.y * moveForce * pVars.playerRB.mass * multiplier);
        if (pVars.x != 0 && horizontalVel.x < pVars.maxVelocity)
            pVars.playerRB.AddForce(transform.right * pVars.x * moveForce * pVars.playerRB.mass * multiplier);
    }
}
