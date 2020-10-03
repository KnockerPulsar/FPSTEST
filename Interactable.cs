using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool activated = false;
    protected Interact pI;

    public virtual void Interact()
    {
        print("Base interactable");
        activated = true;
        pI.interactionPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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
