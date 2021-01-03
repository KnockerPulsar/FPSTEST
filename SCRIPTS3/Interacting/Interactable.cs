using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool activated = false;
   
    public virtual void Interact()
    {
        print("Base interactable");
        activated = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("P");
    //    if (other.CompareTag("Player"))
    //    {
    //        pI = other.GetComponent<Interact>();
    //        if (pI)
    //        {
    //            pI.inRangeOfInteractable = true;
    //            StartCoroutine(pI.TryInteract());
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        activated = false;
        //if (!pI)
        //    pI = other.GetComponent<Interact>();
        //if (pI)
        //{
        //    pI.inRangeOfInteractable = false;
        //    StopCoroutine(pI.TryInteract());
        //    if (pI.interactionPrompt.pickedUpSelf == true)
        //        pI.interactionPrompt.SetActive(false);
        //}
    }

}
