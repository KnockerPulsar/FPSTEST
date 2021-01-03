using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject interactionPrompt;
    public float interactionRange = 4f;
    public bool inRangeOfInteractable = false;

    Interactable interactable;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRangeOfInteractable && interactionPrompt.activeSelf)
        {
            interactable.Interact();
        }
    }

    public IEnumerator TryInteract()
    {
        while (inRangeOfInteractable)
        {
            //print("ass");
            RaycastHit[] hits = Physics.RaycastAll(PlayerVariables.cameraPos, PlayerVariables.cameraFwd, interactionRange);

            if (hits.Length < 1 && interactionPrompt.activeSelf)
                interactionPrompt.SetActive(false);

            Debug.DrawRay(PlayerVariables.cameraPos, PlayerVariables.cameraFwd * interactionRange, Color.red, 1f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Interactable") && !hit.collider.isTrigger)
                {
                    interactable = hit.collider.GetComponentInParent<Interactable>();
                    //print(interactable.name);
                    if (!interactable.activated)
                    {
                        interactionPrompt.SetActive(true);
                    }
                    else
                        interactionPrompt.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            //interactionPrompt.SetActive(true);
            inRangeOfInteractable = true;
            StartCoroutine(TryInteract());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable)
                interactable.activated = false;

            interactionPrompt.SetActive(false);
            inRangeOfInteractable = false;
            StopCoroutine(TryInteract());
        }
    }
}