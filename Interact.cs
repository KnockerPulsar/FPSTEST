using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public PlayerVariables pVars;
    public GameObject interactionPrompt;
    public float interactionRange = 4f;
    public bool inRangeOfInteractable = false;

    Interactable interactible;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactionPrompt.activeSelf)
        {
            interactible.Interact();
        }
    }

    public IEnumerator TryInteract()
    {
        while (inRangeOfInteractable)
        {
            RaycastHit[]  hits = Physics.RaycastAll(pVars.cameraPos, pVars.cameraFwd, interactionRange);

            if (hits.Length < 1 && interactionPrompt.activeSelf)
                interactionPrompt.SetActive(false);

            Debug.DrawRay(pVars.cameraPos, pVars.cameraFwd * interactionRange, Color.red, 1f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Interactable") && !hit.collider.isTrigger)
                {
                    interactible = hit.collider.GetComponentInParent<Interactable>();
                    if (!interactible.activated)
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
}