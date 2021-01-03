using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractableDoor : Interactable
{
    public Key requiredKey = Key.red;
    public Door doorScript;

    public override void Interact()
    {
        if (activated)
            return;

        if ((int)PlayerVariables.HeldKey == (int)requiredKey)
        {
            base.Interact();
            doorScript.Open();
            StartCoroutine(PlayerVariables.ShowText("Door unlocked", 5f));
        }
        else
        {
            string requiredKeyText = PlayerVariables.GetKeyName(requiredKey);
            StartCoroutine(PlayerVariables.ShowText(requiredKeyText + " key required", 5f));
        }
    }
}