using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : BasePickup
{
    public float healAmount = 10f;
    public float activationDist = 0.1f;
    public float upForce = 10f;
    public float approachForce = 3000f;


    bool healed = false;
    Rigidbody rb;
    PlayerHealthManager playerHM;
    Vector3 toPlayerVec;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (pickedUp && !healed)
        {
            toPlayerVec = PlayerVariables.playerPos - transform.position;
            if (toPlayerVec.magnitude < activationDist)
            {
                playerHM.Heal(healAmount);
                pickupMesh.SetActive(false);
                healed = true;
                Destroy(gameObject, pickupSound.length);
            }

            toPlayerVec.Normalize();
            rb.AddForce(toPlayerVec * approachForce * Time.deltaTime);
        }
    }

    public override void InitiatePickup()
    {
        playerHM = PlayerVariables.player.GetComponent<PlayerHealthManager>();
        if (playerHM && playerHM.currentHealth < playerHM.maxHealth)
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.enabled = false;

            rb.useGravity = false;
            rb.AddForce(Vector3.up * 10f * upForce);
            pickedUp = true;
        }
    }
}
