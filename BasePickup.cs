using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePickup : MonoBehaviour
{
    public GameObject pickupMesh;
    public AudioSource audioSource;
    public float pickupDelay = 0.5f;

    protected GameObject player;
    bool active = false;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            if (pickupMesh)
                pickupMesh.SetActive(false);
            if (audioSource)
                audioSource.Play();
            Invoke(nameof(InitiatePickup), pickupDelay);
        }
    }

    public virtual void InitiatePickup() { }


}
