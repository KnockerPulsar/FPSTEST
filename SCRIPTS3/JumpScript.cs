using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for handling the pressedJump ability of the player.
public class JumpScript : MonoBehaviour
{
    public float jumpForce = 30f;           //Used to determine the jump force and thus it's height.
    public LayerMask groundLayer;

    public float maxButtonDelay = 0.2f;
    public AudioSource audioSource;
    public AudioClip landingSound;
    [Range(0, 1)]
    public float landingSoundVolume = 0.5f;
    public float minTimeForLandSound = 0.2f;// The minimum length of time a player should jump/fall for so that the landing sound effect plays
    public bool jumped = false;

    private void Start()
    {
        PlayerVariables.timeSinceUngrounded = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerVariables.pressedJump = Input.GetKeyDown(KeyCode.Space);


        // The counter is reset in the Land function
        if (!PlayerVariables.isGrounded)
            PlayerVariables.timeSinceUngrounded += Time.deltaTime;

        PlayerVariables.prevFrameGrounded = PlayerVariables.isGrounded;
        PlayerVariables.isGrounded = Physics.CheckSphere
            (transform.position - Vector3.up * PlayerVariables.playerCapsule.height / 2f, 0.2f, groundLayer);

        // Checks if the player pressed the space bar (PlayerVariables.pressedJump) and if they're on the ground and if the player is not Wallrunning.
        // If so, initiates a jump.
        if (PlayerVariables.pressedJump &&
            (PlayerVariables.isGrounded || PlayerVariables.timeSinceUngrounded < maxButtonDelay)
            && !PlayerVariables.isWallrunning && !PlayerVariables.paused)
            Jump();

        // If the player was in the air the previous frame and now is on the ground, call the Land function
        if (!PlayerVariables.prevFrameGrounded && PlayerVariables.isGrounded && PlayerVariables.timeSinceUngrounded > minTimeForLandSound)
            StartCoroutine(Land());


    }

    //The jump is dependant on the multiplier and the player's mass and the max velocity the player can walk/run.
    void Jump()
    {
        jumped = true;
        //PlayerVariables.timeSinceUngrounded = float.MaxValue;
        PlayerVariables.playerRB.AddForce(Vector3.up * jumpForce);
    }

    // Currently just plays a sound effect
    IEnumerator Land()
    {
        PlayerVariables.timeSinceUngrounded = 0;
        jumped = false;
        if (audioSource && landingSound)
        {
            float tempPitch = audioSource.pitch;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(landingSound, landingSoundVolume);

            yield return new WaitForSeconds(landingSound.length);
            audioSource.pitch = tempPitch;
        }
    }
}
