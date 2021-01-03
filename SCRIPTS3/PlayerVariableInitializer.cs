using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Responsible for initializing the PlayerVariables variables and maintaining them.
public class PlayerVariableInitializer : MonoBehaviour
{
    public TextMeshProUGUI UITextScript;
    public Animator UITextAnimator;
    public AnimationClip UITextAnimation;

    public Animator KatanaAnimator;

    //Caches commonly used/ shares variables at the start of the game / level.
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        PlayerVariables.playerCapsule = GetComponent<CapsuleCollider>();

        //PlayerVariables.player = GameObject.FindGameObjectWithTag("Player");


        PlayerVariables.secondsSinceLevelStart = 0;
        PlayerVariables.UITextScript = UITextScript;
        PlayerVariables.UITextAnimator = UITextAnimator;
        PlayerVariables.UITextAnimation = UITextAnimation;

        PlayerVariables.KatanaAnimator = KatanaAnimator;
    }


    //Checks if the player on the ground, if the spacebar is pressed or held, if any of the WASD keys is pressed and maintains the time since spawn.
    private void Update()
    {
        //PlayerVariables.spaceHeld = Input.GetKey(KeyCode.Space);

        PlayerVariables.secondsSinceLevelStart += Time.deltaTime;

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position - Vector3.up * 1.5f, 0.5f);
    //}

}
