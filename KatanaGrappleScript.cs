using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Responsible for the grappling ability on the katana.
public class KatanaGrappleScript : MonoBehaviour
{
    public PlayerVariables pVars;                    //A container responsible for storing commonly used / shared variables.
    public Material lineRendererMaterial;            //Stores the material for the grappling line.
    public float lineRendererWidth = 0.25f;          //Stores the width for the grappling line.
    public GameObject grappleOriginObj;              //Stores the katana's game object.
    public float grappleRange = 150f;                //The maximum range a player can grapple, used in the range check and feedback.
    public float minFeedbackDist = 20f;              //The minimum distance the feedback object should spawn.
    public float jointSpring = 3f;                   //The joint spring for the grappling line, determines the forces affecting the player.
    public float jointDamper = 1.5f;                 //The joint damper for the grappling line.
    public GameObject feedbackObj;                   //The object shown to indicate the player can grapple.

    Vector3 grappleOrigin;                           //Where the grapple should start, should be at whatever we're grappling from (A weapon's muzzle for example)
    Vector3 grapplePoint;                            //The point where the raycast hits, used for the grappling spring joint
    SpringJoint joint;                               //The spring joint used to affect the player's movement
    LineRenderer line;                               //A line renderer used to provide visual feedback as the spring joint doesn't render anything
    Quaternion baseGunRotation;                      //Used for returning the gun to its original rotation after grappling
    WeaponSystemScript wepSys;                       //The weapon system, used for checking if the current weapon is the katana and its state (idle, attacking grappling).
    GameObject spawnedObj;                           //A reference to the spawned feedback object, used to move/destory it.
    float GrappleSlerpVal = 0;                       //A float used to interpolate between the katana's rotation and it's original rotation to return it to it's initial rotation after grappling.
    Animator katanaAnimator;                         //The animator of the katana, used to disable and enable itself so that animations can't play while grappling.
    RaycastHit[] hits;                               //The hits of the grapple raycast.

    // Start is called before the first frame update
    //Caches the katana's starting rotation, the weapon system, the line component, the line material and the animator.
    void Start()
    {
        baseGunRotation = transform.rotation;
        wepSys = pVars.player.GetComponent<WeaponSystemScript>();
        line = GetComponent<LineRenderer>();
        line.material = lineRendererMaterial;
        katanaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Grapples while the player presses the right mouse button
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Grapple();
        }
        //Stops grappling once the player lifts the right mouse button
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            EndGrapple();
            GrappleSlerpVal = 0;
        }


        //If the joint exists / the player is grappling, updates the grapple points and rotates the gun
        if (joint)
            UpdateGrappleLocation();
        //Otherwise it resets the guns rotation to it's base rotation relative to the player's capsule
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, pVars.playerCam.transform.rotation * baseGunRotation, GrappleSlerpVal);
            if (GrappleSlerpVal >= 1 && katanaAnimator.enabled == false)
            {
                print("Animator activated");
                katanaAnimator.enabled = true;
                GetComponent<BaseWeaponScript2>().currentWeaponState = (int)WeaponState.idle;
            }
        }

        //Spawns/moves the feedback object
        ProvideFeedback();
    }

    private void FixedUpdate()
    {
        if (!pVars.isGrappling && GrappleSlerpVal < 1f)
        {
            GrappleSlerpVal += Time.fixedDeltaTime / (0.25f);
            //print(GrappleSlerpVal);
        }
    }

    //Checks if the current weapon is the katana and it is idle and if so, updates its state to grappling and disables the animator,
    //updates the player's grappling boolean, get's the line renderer start point and casts a ray to determine whether the player can
    //grapple, if so, adds a joint to the player with the other end at the grappling point, and finally, positions the lineRenderer.
    private void Grapple()
    {
        print("G");
        if (wepSys.hasWeapon[(int)WeaponCodes.katana] && wepSys.currentWeaponScript.currentWeaponState == (int)WeaponState.idle)
        {
            print("Grapple");
            //Casts a ray and sees if it intersects with anything
            hits = Physics.RaycastAll(pVars.playerCam.transform.position, pVars.playerCam.transform.forward.normalized, grappleRange);
            if (hits.Length < 1) return;

            wepSys.currentWeaponScript.currentWeaponState = (int)WeaponState.grappling;
            katanaAnimator.enabled = false;
            pVars.isGrappling = true;
            line.enabled = true;
            grappleOrigin = grappleOriginObj.transform.position;

            grapplePoint = hits[0].point;                           //Finds the first intersection
            joint = pVars.player.AddComponent<SpringJoint>();       //Adds a spring joint to the player
            joint.autoConfigureConnectedAnchor = false;             //No idea what this does
            joint.connectedAnchor = grapplePoint;

            //Setting some properties for the springjoint
            joint.spring = jointSpring;
            joint.damper = jointDamper;

            Vector3[] lineRendererPositions = { grappleOrigin, grapplePoint };   //Sets up the positions for the line renderer
            line.SetPositions(lineRendererPositions);    //Sets the positions for the line renderer
        }
    }

    private void UpdateGrappleLocation()
    {
        grappleOrigin = grappleOriginObj.transform.position;         //Updates the grapple origin
        Vector3 dir = (grapplePoint - grappleOrigin).normalized;     //Finds the vector between the grapple point and origin


        //Calculates the new direction for the gun
        //Interpolates between the previous and current rotations based on the velocity
        Quaternion lookdir = Quaternion.LookRotation(dir) * baseGunRotation;
        transform.forward = Vector3.Slerp(transform.forward, -dir, Time.deltaTime * pVars.playerRB.velocity.magnitude / 2f);


        //Updates the line renderer positions and sets them
        Vector3[] lineRendererPositions = { grappleOrigin, grapplePoint };
        line.SetPositions(lineRendererPositions);

    }

    //Reset's the katana's state to idle, disables the line renderer, updates the player's grappling boolean and destroys the joint.
    private void EndGrapple()
    {
        if (wepSys.currentWeaponScript)
        {
            wepSys.currentWeaponScript.currentWeaponState = (int)WeaponState.idle;
            line.enabled = false;
            pVars.isGrappling = false;
            Destroy(joint);
            line.enabled = false;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
        }

    }

    //Does a raycast using the player camera's position, forward vector and the grappling range
    //If it hits something, calculates the distance and checks if there is no feedback object.
    //If so it spawns one and scales it depending on the distance.
    //Otherwise it checks if the distance isn't in the range of grappling (too far or close) and if the player isn't grappling,
    //If so, it destorys whatever object is spawned.
    //if the player is grappling, sets the object's position to the grapple point and scales it appropriately.
    //If the player is not grappling, finds where the raycast hit, positions the object there and scales it.
    private void ProvideFeedback()
    {
        RaycastHit[] hit = Physics.RaycastAll(pVars.playerCam.transform.position, pVars.playerCam.transform.forward, grappleRange);
        float dist = 151f;
        if (hit.Length > 0)
            dist = Vector3.Distance(hit[0].point, pVars.playerCam.transform.position);

        if (!spawnedObj)
        {
            if (dist < grappleRange && dist > minFeedbackDist)
            {
                spawnedObj = Instantiate(feedbackObj, hit[0].point, Quaternion.Euler(0, 0, 0));
                spawnedObj.transform.localScale = Vector3.one * Mathf.Sqrt(dist) / 2f;
            }
        }
        else
        {
            if ((dist > grappleRange || dist < minFeedbackDist ) && !pVars.isGrappling)
                Destroy(spawnedObj);
            else
            {
                if (pVars.isGrappling)
                {
                    spawnedObj.transform.position = grapplePoint;
                    spawnedObj.transform.localScale = Vector3.one * Mathf.Sqrt(Vector3.Distance(grapplePoint, grappleOrigin)) / 2f;
                }
                else
                {
                    spawnedObj.transform.position = hit[0].point;
                    spawnedObj.transform.localScale = Vector3.one * Mathf.Sqrt(dist) / 2f;
                }
            }
        }
    }
}

