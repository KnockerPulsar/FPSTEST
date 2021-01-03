using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePickup : MonoBehaviour
{
    public GameObject pickupMesh;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    // How long should the object stay before destroying itself?
    // If the pickup has a sound effect, it will destroy after that sound effect's length instead
    // Otherwise, it will use the below float.
    public float destroyDelay = 0.5f; 

    // How long to wait after hiding the mesh to call the InitiatePickup function
    public float pickupDelay = 0.5f;
    public bool hideMeshOnPickup = true;
    public bool playSoundOnPickup = true;

    [Tooltip("If true, calls InitiatePickup when the players pickup sphere trigger overlaps the object, otherise it calls it on overlap of the collision capsule ")]
    public bool pickUpOnTrigger = true;

    protected GameObject player;
    protected bool pickedUp = false;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickUpOnTrigger && other.isTrigger)
                PickUp(other.gameObject);
            else if (!pickUpOnTrigger && !other.isTrigger)
                PickUp(other.gameObject);
        }
    }

    public virtual void InitiatePickup() { }

    private void PickUp(GameObject playerGO)
    {
        if (pickedUp) return;
        pickedUp = true;
        player = playerGO;
        if (pickupMesh && hideMeshOnPickup)
            pickupMesh.SetActive(false);

        if (audioSource && playSoundOnPickup && pickupSound)
            audioSource.PlayOneShot(pickupSound);

        Invoke(nameof(InitiatePickup), pickupDelay);

        Destroy(gameObject, destroyDelay);
    }
}
