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

    //Using FixedUpdate for consistent movement.
    //If the player isn't on the ground, checks if the player is moving into a wall and if so, stops movement
    //This is to prevent players from latching onto unwanted walls.
    void FixedUpdate()
    {
        if (MovementCheck())
            Move();
    }


    //Fires a raycast based on the WASD keys the player is pressing, returns true if the player has no object in that direction or there's a climbabl/latchable object.
    bool MovementCheck()
    {
        if (pVars.isGrounded[1])
            return true;

        //Raycast based on the pressed movement keys
        //If there is no object / the object is not climable or latchable, return false, otherwise return true
        Vector3 checkDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            checkDir += transform.forward;
        if (Input.GetKey(KeyCode.A))
            checkDir += transform.right * -1;
        if (Input.GetKey(KeyCode.S))
            checkDir += transform.forward * -1;
        if (Input.GetKey(KeyCode.D))
            checkDir += transform.right;

        movementCheckHits = Physics.RaycastAll(transform.position, checkDir, 3f);

        //Uncomment when implementing wall latching
        if (movementCheckHits.Length == 0)
            return true;
        else if (movementCheckHits.Length > 0 && movementCheckHits[0].collider.CompareTag("Climbable") /*|| movementCheckHits[0].collider.CompareTag("Latchable")*/)
            return true;
        else
            return false;
    }

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
                pVars.playerRB.AddForce(moveForce * pVars.playerRB.mass * transform.right * Time.deltaTime * -Mag.x * countermovementMultiplier);
            if (y == 0 && Mag.y != 0)
                pVars.playerRB.AddForce(moveForce * pVars.playerRB.mass * transform.forward * Time.deltaTime * -Mag.y * countermovementMultiplier);
        }

    }

    //Applies the movement and countermovement.
    void Move()
    {
        horizontalVel = FindVelRelativeToLook();

        if (!pVars.isCrouching)
            CounterMovement(pVars.x, pVars.y, horizontalVel);

        if (pVars.y != 0 && horizontalVel.y < pVars.maxVelocity)
            pVars.playerRB.AddForce(transform.forward * pVars.y * moveForce * pVars.playerRB.mass * Time.fixedDeltaTime);
        if (pVars.x != 0 && horizontalVel.x < pVars.maxVelocity)
            pVars.playerRB.AddForce(transform.right * pVars.x * moveForce * pVars.playerRB.mass * Time.fixedDeltaTime);
    }
}
