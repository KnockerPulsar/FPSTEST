using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1Manager : MonoBehaviour
{
    public GameObject UIText;
    public AnimationClip anim;
    public string InitialTut = "";
    public float hideTextAfter = 20f;

    public bool health = false;
    public bool canDash = false;
    public bool canSlide = false;
    public bool canWallrun = false;
    public bool canClimb = false;
    public bool canJump = false;

    MonoBehaviour[] playerScripts;
    Animator UITextAnimator;
    TextMeshProUGUI textComp;


    // Start is called before the first frame update
    void Start()
    {
        playerScripts = GameObject.FindGameObjectWithTag("Player").GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mono in playerScripts)
        {
            if (mono.GetType() == typeof(DashScript))
            {
                if (canDash == false)
                    mono.enabled = false;
            }
            else if (mono.GetType() == typeof(SlideScript))
            {
                if (canSlide == false)
                    mono.enabled = false;
            }

            else if (mono.GetType() == typeof(WallrunningScript))
            {
                if (canWallrun == false)
                    mono.enabled = false;
            }

            else if (mono.GetType() == typeof(WallclimbingScript))
            {
                if (canClimb == false)
                    mono.enabled = false;
            }
            else if (mono.GetType() == typeof(JumpScript))
            {
                if (canJump == false)
                    mono.enabled = false;
            }
            else if (mono.GetType() == typeof(PlayerHealthManager))
            {
                if (health == false)
                    mono.enabled = false;
            }
        }

        UITextAnimator = UIText.GetComponent<Animator>();
        textComp = UIText.GetComponent<TextMeshProUGUI>();

        StartCoroutine(TutorialText());
    }
    IEnumerator TutorialText()
    {

        yield return new WaitForSeconds(2f);

        UIText.SetActive(true);
        textComp.text = InitialTut;
        UITextAnimator.Play(anim.name + "Fwd");

        yield return new WaitForSeconds(anim.length + hideTextAfter);
        print("b");

        UITextAnimator.Play(anim.name + "Bck");

        yield return new WaitForSeconds(anim.length);

        UIText.SetActive(false);
    }
}
