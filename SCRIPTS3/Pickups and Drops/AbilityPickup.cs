using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPickup : BasePickup
{
    public bool Wallclimb = false;
    public bool Slide = false;
    public bool Dash = false;
    public bool Wallrun = false;
    public bool Jump = false;

    public TMP_Text PickupName;
    [TextArea(5,5)]
    public string Desc = "";
    public Light attachedLight;
    public float lightFadeSpeed = 4f;

    string pickupName = "";

    private void Start()
    {
        PickupName = GetComponentInChildren<TMP_Text>();
        if (Wallclimb)
            pickupName += nameof(Wallclimb);
        if (Slide)
            pickupName += nameof(Slide);
        if (Dash)
            pickupName += nameof(Dash);
        if (Wallrun)
            pickupName += nameof(Wallrun);
        if (Jump)
            pickupName += nameof(Jump);

        PickupName.SetText(pickupName);
    }

    public override void InitiatePickup()
    {

        if (Wallclimb)
            GameObject.FindGameObjectWithTag("Player").GetComponent<WallclimbingScript>().enabled = true;

        else if (Slide)
            GameObject.FindGameObjectWithTag("Player").GetComponent<SlideScript>().enabled = true;

        else if (Dash)
            GameObject.FindGameObjectWithTag("Player").GetComponent<DashScript>().enabled = true;

        else if (Wallrun)
            GameObject.FindGameObjectWithTag("Player").GetComponent<WallrunningScript>().enabled = true;

        else if (Jump)
            GameObject.FindGameObjectWithTag("Player").GetComponent<JumpScript>().enabled = true;


        // Rotating the whole gameobject to face the player
        gameObject.transform.rotation = Quaternion.LookRotation(-GameObject.FindGameObjectWithTag("Player").transform.right);

        SimpleRotateFloatScript SRS = GetComponentInChildren<SimpleRotateFloatScript>();
        if (SRS)
            SRS.enabled = false;
        PickupName.enabled = false;


        StartCoroutine(PlayerVariables.ShowText(Desc, destroyDelay - 2 * PlayerVariables.UITextAnimation.length)); ;

        if (attachedLight)
            StartCoroutine(FadeLight());
    }

    IEnumerator FadeLight()
    {
        while (attachedLight.intensity > 0)
        {
            attachedLight.intensity -= lightFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
