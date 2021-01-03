using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedGravityField : MonoBehaviour
{
    public float addedGravity = 50f;

    Rigidbody playerRB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false && other.CompareTag("Player"))
            playerRB = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger == false && other.CompareTag("Player"))
            playerRB.velocity += -transform.up * addedGravity;
    }
}
