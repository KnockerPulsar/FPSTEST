using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class KatanaWeaponScript : BaseWeaponScript2
{
    public PlayerVariables pVars;
    public AnimationClip katanaShooting;
    Animator katanaAnimator;
    BoxCollider hitBox;
    PlayerMovementScript playerMovement;
    Vector3 dashFinal;
    Camera playerCam;
    RaycastHit[] hitObjects;
    public GameObject hitMarker;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
        katanaAnimator = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
    }

    public override void Shoot()
    {
        if (WeaponState_Script == (int)WeaponState.idle)
        {
            //base.fireRate = 1f / katanaShooting.length;
            base.WeaponState_Script = (int)WeaponState.shooting;
            //katanaAnimator.SetInteger("WeaponState_Anim", WeaponState_Script);
            //print(base.fireRate);
            base.fireRate = 1f / katanaShooting.length;


            //TODO: Fix this or remove it
            //If a dash gets the player in the air (off a ledge for example), the player is launched at a very high speed. Also, any vertical movment cause massive velocity changes due to the lack of countermovement 
            //A possible solution is to implement a separate countermovement function just for this.
            //KatanaDash();
            hitObjects = Physics.RaycastAll(pVars.playerCam.transform.position, pVars.playerCam.transform.forward, base.range);
            if(hitObjects.Length > 0 && hitObjects[0].transform.gameObject.CompareTag("Enemy"))
            {
                print("Enemy hit");
                hitMarker.GetComponent<Animator>().SetTrigger("Show");
                Destroy(hitObjects[0].transform.gameObject);
            }

            katanaAnimator.Play("KatanaSlice");
            katanaAnimator.Play("KatanaSlice", -1, 0);
            Invoke(nameof(EndShoot), 1f / base.fireRate);
        }
    }

    void EndShoot()
    {
        base.WeaponState_Script = (int)WeaponState.idle;
        katanaAnimator.SetInteger("WeaponState_Anim", WeaponState_Script);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (WeaponState_Script == (int)WeaponState.shooting)
        {
            print("Hit something");
            if (other.gameObject.CompareTag("Enemy"))
            {
                print("Enemy hit");
                Destroy(other.gameObject);
            }
            else
                print("Other hit");
        }
    }

    public void KatanaDash()
    {
        playerMovement.isDashing = true;
        Vector3 dashForward = Vector3.zero;
        Vector3 cameraForward = new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z);

        if (playerMovement.grounded[1])
            dashForward = cameraForward * playerMovement.maxVelocity * playerMovement.DashVelocityMultiplier * 4f;
        else
            dashForward = cameraForward * playerMovement.maxVelocity * playerMovement.DashVelocityMultiplier / 8f;


        dashFinal = dashForward;
        print(dashFinal);

        playerMovement.rBody.velocity += dashFinal;
        Invoke(nameof(EndKatanaDash), playerMovement.DashLengthInSeconds);
    }

    void EndKatanaDash()
    {
        playerMovement.isDashing = false;
        playerMovement.rBody.velocity -= dashFinal / 20f;
    }


}
