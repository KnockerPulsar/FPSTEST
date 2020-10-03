using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public PlayerVariables pVars;
    public GameObject pickupMesh;
    public AudioSource audioSource;
    public float healAmount = 10f;
    public float activationDist = 0.1f;
    public float upForce = 10f;
    public float approachForce = 3000f;
    public float pickupDelay = 0.5f;


    bool active = false;
    bool healed = false;
    Rigidbody rb;
    PlayerHealthManager playerHM;
    Vector3 toPlayerVec;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        activationDist = pVars.playerCap.radius;
    }

    private void Update()
    {
        if (active && !healed)
        {
            toPlayerVec = pVars.playerPos - transform.position;
            if (toPlayerVec.magnitude < activationDist)
            {
                playerHM.Heal(healAmount);
                pickupMesh.SetActive(false);
                healed = true;
                audioSource.Play();
                Destroy(gameObject, audioSource.clip.length);
            }

            toPlayerVec.Normalize();
            rb.AddForce(toPlayerVec * approachForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(StartPickup), pickupDelay);
        }

    }

    void StartPickup()
    {
        playerHM = pVars.player.GetComponent<PlayerHealthManager>();
        if (playerHM && playerHM.currentHealth < playerHM.maxHealth)
        {
            BoxCollider bc = GetComponentInChildren<BoxCollider>();
            if (bc)
                bc.enabled = false;

            rb.useGravity = false;
            rb.AddForce(Vector3.up * 10f * upForce);
            active = true;

        }

    }
}
