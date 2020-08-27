using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;


//Responsible for the wallclimbing ability of the player.
public class WallrunningScript : MonoBehaviour
{
    public PlayerVariables pVars;                               //A container responsible for storing commonly used / shared variables.
    public float wallrunForceMultiplier = 2000f;                //The force applied when jumping off a wall.
    public Vector3 wallrunNormal;                               //The normal vector at the point where the wallrun check hits.
 
    Vector3 checkDir = Vector3.zero;                            //The wallrun check raycast direction.
    RaycastHit[] movementCheckHits;                             //The result of the wallrun check raycast.
    float wallrunLeanInDeg = 15f;                               //The maximum camera tilt when wallrunning.
    float leanPercentage = 0;                                   //The variable used to tilt the camera.
    Vector3 crossProduct;                                       //the cross product of the wallrunNormal and the up vector, if it's in the opposite direction of the player, it gets flipped.
    float dotProduct;                                           //The dot product of crossProduct and the players forward vector, determines if crossProduct will be flipped.
    float dir = 0;                                              //The direction the player is approaching the wall.

    // Update is called once per frame
    //Checks if the player can wallrun when the player is in the air.
    void FixedUpdate()
    {
        if (!pVars.isGrounded[1])
            Wallrun();
    }


    // Checks if there is a wall in the direction the player is moving towards, if there is, returns the normal vector where the raycast hit.
    Vector3 wallrunCheck()
    {
        checkDir = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            checkDir += transform.right * -1;
        if (Input.GetKey(KeyCode.D))
            checkDir += transform.right;

        //Debug.Log("WallrunCheck");

        movementCheckHits = Physics.RaycastAll(transform.position - new Vector3(0, pVars.playerCapsule.height, 0), checkDir, 3f);

        foreach (RaycastHit hit in movementCheckHits)
            if (hit.transform.gameObject.CompareTag("Wallrunnable"))
                movementCheckHits[0] = hit;

        //Debug.DrawRay(transform.position, checkDir * 6f, Color.red, 3);

        if (movementCheckHits.Length > 0 && movementCheckHits[0].collider.CompareTag("Wallrunnable"))
            return movementCheckHits[0].normal;
        else
            return Vector3.zero;
    }

    //If the wallrunCheck passes and the player isn't wallrunning, sets the players velocity to a scaled version of the crossProduct (if in the opposite direction of the player, it gets flipped).
    //Also causes the player's camera to tilt while wallrunning and return once off the wall.
    //If the player is already wallrunning and presses the spacebar, pushes the player up and away from the wall.
    void Wallrun()
    {
        //If there's a wall where the player is moving sideways
        wallrunNormal = wallrunCheck();
        wallrunNormal.y = 0;

        pVars.wallrunNormal = wallrunNormal;

        if (Input.GetKey(KeyCode.Space) && pVars.isWallrunning)
        {
            pVars.isWallrunning = false;
            JumpOff();
        }

        else if (wallrunNormal != Vector3.zero)
        {
            pVars.isWallrunning = true;
            crossProduct = Vector3.Cross(wallrunNormal, Vector3.up);
            dotProduct = Vector3.Dot(crossProduct, transform.forward); //Finds out whether the cross product will move the player forward or backward

            if (Input.GetKey(KeyCode.A))
                dir = -1;
            else if (Input.GetKey(KeyCode.D))
                dir = 1;
            wallrunLeanInDeg = 15 * dir; //Finds out the lean direction



            //Rotates the camera about its forward axis by the lean amount
            if (leanPercentage < 1)
                leanPercentage += 0.04f;

            //Corrects the wallrun direction if it will push the player back instead of forward
            if (dotProduct < 0)
                crossProduct *= -1;

            crossProduct.Normalize();
            pVars.playerRB.velocity = crossProduct * 40f;
            pVars.playerRB.velocity -= new Vector3(0, pVars.playerRB.velocity.y, 0); //Nullifies any vertical velocity to prevent falling

        }
        else
        {
            pVars.isWallrunning = false;
            if (leanPercentage > 0.04)
                leanPercentage -= 0.04f;
        }
        //print(leanPercentage);
        pVars.playerCamParent.transform.localRotation = Quaternion.Euler(0, 0, wallrunLeanInDeg * leanPercentage);
    }


    //Applies forces up and away from the wall.
    void JumpOff()
    {
        pVars.playerRB.AddForce(wallrunNormal.normalized * pVars.playerRB.mass * wallrunForceMultiplier);
        pVars.playerRB.AddForce(Vector3.up * pVars.playerRB.mass * wallrunForceMultiplier / 1.5f);
    }
}
