using UnityEngine;
using System.Collections;

//Responsible for the wallclimbing ability of the player.
public class WallrunningScript : MonoBehaviour
{
    public float wallrunSpeed = 20f;
    public float wallrunCheckDist = 2f;
    public float wallrunForceMultiplier = 2000f;                //The force applied when pressedJump off a wall.
    public float movementDelay = 0.1f;
    public float wallrunStepSFXDelay = 0.3f;

    [HideInInspector] public Vector3 wallrunNormal;             //The normal vector at the point where the wallrun check hits.


    Vector3 wallUpVector = Vector3.zero;
    Vector3 checkDir = Vector3.zero;                            //The wallrun check raycast direction.
    RaycastHit movementCheckHit;                                //The result of the wallrun check raycast.
    float wallrunLeanInDeg = 15f;                               //The maximum camera tilt when wallrunning.
    float leanPercentage = 0;                                   //The variable used to tilt the camera.
    Vector3 crossProduct;                                       //the cross product of the wallrunNormal and the up vector, if it's in the opposite direction of the player, it gets flipped.
    float dotProduct;                                           //The dot product of crossProduct and the players forward vector, determines if crossProduct will be flipped.
    float dir = 0;                                              //The direction the player is approaching the wall.
    public float normalWalkingSFXDelay;

    public void Start()
    {
        normalWalkingSFXDelay = PlayerVariables.walkScript.delayBetweenFootsteps;
    }

    // Update is called once per frame
    //Checks if the player can wallrun when the player is in the air.
    void FixedUpdate()
    {
        if (!PlayerVariables.isGrounded)
            Wallrun();

        //print(leanPercentage);
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
        if (checkDir != Vector3.zero)
        {
            Physics.Raycast(transform.position, checkDir, out movementCheckHit, wallrunCheckDist);

            Debug.DrawRay(transform.position, checkDir * wallrunCheckDist, Color.red, 3);

            if (movementCheckHit.collider && movementCheckHit.collider.CompareTag("Wallrunnable"))
            {
                //The negative is because the wallrun prefab's default up vector points down.
                wallUpVector = -movementCheckHit.collider.transform.up;
                return movementCheckHit.normal;

            }
            else
            {
                PlayerVariables.isWallrunning = false;
                if (leanPercentage >= 0.04)
                    StartCoroutine(WallrunCleanup());
                return Vector3.zero;
            }
        }
        else
        {
            PlayerVariables.isWallrunning = false;
            if (leanPercentage >= 0.04)
                StartCoroutine(WallrunCleanup());
            return Vector3.zero;
        }
    }

    //If the wallrunCheck passes and the player isn't wallrunning, sets the players velocity to a scaled version of the crossProduct (if in the opposite direction of the player, it gets flipped).
    //Also causes the player's camera to tilt while wallrunning and return once off the wall.
    //If the player is already wallrunning and presses the spacebar, pushes the player up and away from the wall.
    void Wallrun()
    {

        //If there's a wall where the player is moving sideways
        wallrunNormal = wallrunCheck();
        wallrunNormal.y = 0;

        PlayerVariables.wallrunNormal = wallrunNormal;

        if (Input.GetKey(KeyCode.Space) && PlayerVariables.isWallrunning)
        {
            StartCoroutine(JumpOff());
        }

        else if (wallrunNormal != Vector3.zero)
        {
            PlayerVariables.walkScript.delayBetweenFootsteps = 0;
            PlayerVariables.isWallrunning = true;
            crossProduct = Vector3.Cross(wallrunNormal, wallUpVector);
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
            PlayerVariables.playerRB.velocity = crossProduct * wallrunSpeed;
        }
        PlayerVariables.playerCamParent.transform.localRotation = Quaternion.Euler(0, 0, wallrunLeanInDeg * leanPercentage);
    }


    //Applies forces up and away from the wall.
    IEnumerator JumpOff()
    {
        StartCoroutine(WallrunCleanup());

        PlayerVariables.isWallrunning = false;
        PlayerVariables.walkScript.enabled = false;

        PlayerVariables.playerRB.AddForce(wallrunNormal.normalized * PlayerVariables.playerRB.mass * wallrunForceMultiplier);
        PlayerVariables.playerRB.AddForce(Vector3.up * PlayerVariables.playerRB.mass * wallrunForceMultiplier / 2f);

        yield return new WaitForSeconds(movementDelay);

        PlayerVariables.walkScript.enabled = true;
    }

    // Should reset the camera rotation and reset the walking SFX delay
    IEnumerator WallrunCleanup()
    {
        if (PlayerVariables.walkScript.delayBetweenFootsteps != normalWalkingSFXDelay)
            PlayerVariables.walkScript.delayBetweenFootsteps = normalWalkingSFXDelay;
        while (leanPercentage > 0.038 && !PlayerVariables.isWallrunning)
        {
            leanPercentage -= 0.04f;
            PlayerVariables.playerCamParent.transform.localRotation = Quaternion.Euler(0, 0, wallrunLeanInDeg * leanPercentage);
            yield return new WaitForFixedUpdate();
        }
    }
}
