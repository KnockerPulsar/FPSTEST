using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveInteractTest : Interactable
{
    public bool dissolved = false;

    public override void Interact()
    {
        base.Interact();

        DissolveMatScript disMatScript = GetComponent<DissolveMatScript>();
        if (!dissolved)
        {
            dissolved = true;
            disMatScript.StartDissolvingOut();
        }
        else
        {
            dissolved = false;
            disMatScript.StartDissolvingIn();
        }
    }
}
