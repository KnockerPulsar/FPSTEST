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
    public AnimationClip afterPickupTextAnim;
    public GameObject UITextGameObject;
    public string Desc = "";

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
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<WallclimbingScript>().enabled = true;
        }
        else if (Slide)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SlideScript>().enabled = true;

        }
        else if (Dash)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<DashScript>().enabled = true;
        }
        else if (Wallrun)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<WallrunningScript>().enabled = true;
        }
        else if (Jump)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<JumpScript>().enabled = true;
        }

        gameObject.transform.rotation = Quaternion.LookRotation(-GameObject.FindGameObjectWithTag("Player").transform.right);

        GetComponent<SimpleRotationScript>().enabled = false;
        PickupName.enabled = false;


        if (UITextGameObject)
        {
            if (UITextGameObject.activeSelf == false)
                UITextGameObject.SetActive(true);
            TextMeshProUGUI UIText = UITextGameObject.GetComponent<TextMeshProUGUI>();
            UIText.SetText(Desc);
            Animator afterPickupAnimator = UIText.GetComponent<Animator>();
            afterPickupAnimator.Play(afterPickupTextAnim.name);
            afterPickupAnimator.Play(afterPickupTextAnim.name,0,0);
        }
        else
            print("!UITGO");

        Destroy(gameObject, afterPickupTextAnim.length);
    }

    public void WASDMessage()
    {

    }
}
