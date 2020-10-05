using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool activated = false;
    public float interactionTriggerRadius = 8f;
    protected Interact pI;

    SphereCollider interactionTrigger;

    private void Start()
    {
        interactionTrigger = GetComponent<SphereCollider>();
        if (!interactionTrigger)
        {
            interactionTrigger = gameObject.AddComponent<SphereCollider>();
            interactionTrigger.isTrigger = true;
            interactionTrigger.radius = interactionTriggerRadius;
        }
    }

    public virtual void Interact()
    {
        print("Base interactable");
        activated = true;
        pI.interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("P");
        if (other.CompareTag("Player"))
        {
            pI = other.GetComponent<Interact>();
            if (pI)
            {
                pI.inRangeOfInteractable = true;
                StartCoroutine(pI.TryInteract());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        activated = false;
        pI.inRangeOfInteractable = false;
        StopCoroutine(pI.TryInteract());
        if (pI.interactionPrompt.activeSelf == true)
            pI.interactionPrompt.SetActive(false);
    }

}
