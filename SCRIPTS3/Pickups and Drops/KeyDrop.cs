using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : BasePickup
{
    public Key addedKey = Key.red;
    public Light attachedLight;
    public float lightFadeSpeed = 1f;
    public override void InitiatePickup()
    {
        PlayerVariables.HeldKey = addedKey;
        string KeyText = PlayerVariables.GetKeyName(addedKey);

        if (attachedLight)
            StartCoroutine(FadeLight());

        StartCoroutine(PlayerVariables.ShowText(KeyText + " key acquired!", 5f - 2 * PlayerVariables.UITextAnimation.length));
    }

    IEnumerator FadeLight()
    {
        while (attachedLight.intensity > 0)
        {
            attachedLight.intensity -= lightFadeSpeed*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
