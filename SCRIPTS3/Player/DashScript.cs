using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Responsible for handling the player's dash ability.
public class DashScript : MonoBehaviour
{
                         //A container responsible for storing commonly used / shared variables.
    public float DashVelocityMultiplier = 1f;        //Used when calculating the dash velocity.
    public float DashCooldown = 1f;                  //How long before the dash refills.
    public float DashLengthInSeconds = 0.2f;         //How long before the player's velocity reset to it's pre-dash value.
    public float MaxDashes = 1f;                     //The max number of dashes a player can do in a row.
    public Image DashIndicator;                      //The UI image used to indicate that a player has a dash charge.
    Vector3 dashFinal;                               //The final vector for the dash
    Vector3 velocityBeforeDash;                      //Stores the player's velocity before dashing to reset after that dash length.

    Vector3 dashForward;                             //Calculates the dash's vector in the forward direction on the player.
    Vector3 dashSide;                                //Same as dashForward but for the side
    Vector3 playerForward;                           //The forward vector of the player, used to make the code a little cleaner.
    Vector3 playerRight;                             //The right vector of the player.

    // Start is called before the first frame update
    void Start()
    {
        if (!DashIndicator)
            DashIndicator = GameObject.Find("Dash Indicator").GetComponent<Image>();
        PlayerVariables.isDashing = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Dash();
    }

    //If the player presses the left shift and is able to dash, checks if the player is pressing any WASD key.
    //If so, updates the player's dash boolean, stores the player's velocity before the dash, calculates the forward and side components of the dash,
    //adds both components, checks if the player is dashing diagonally and adjusts the dash if so. Finally, adds the dash velocity to the player's velocity
    //and resets the player's velocity after the dash length, when the cooldown is over, show's the indicator and reset's the player's dash boolean.

    public void Dash()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !PlayerVariables.isDashing)
        {

            if (PlayerVariables.x != 0 || PlayerVariables.y != 0)
            {
                PlayerVariables.isDashing = true;

                velocityBeforeDash = PlayerVariables.playerRB.velocity;
                DashIndicator.enabled = false;

                playerForward = transform.forward.normalized;
                playerRight = transform.right.normalized;

                dashSide = playerRight * PlayerVariables.x * PlayerVariables.maxVelocity * DashVelocityMultiplier;
                dashForward = playerForward * PlayerVariables.y * PlayerVariables.maxVelocity * DashVelocityMultiplier;

                dashFinal = (dashForward + dashSide); /*+ new Vector3(0, PlayerVariables.playerRBody.velocity.y, 0)*/;
                //print(dashFinal);

                if (PlayerVariables.x != 0 && PlayerVariables.y != 0)
                    dashFinal /= (DashVelocityMultiplier);

                PlayerVariables.playerRB.velocity += dashFinal;

                Invoke(nameof(EndDash), DashLengthInSeconds);
                Invoke(nameof(ResetDash), DashCooldown);
            }
        }
    }

    void EndDash()
    {
        PlayerVariables.playerRB.velocity = velocityBeforeDash.magnitude * PlayerVariables.playerRB.velocity.normalized;
    }

    private void ResetDash()
    {
        PlayerVariables.isDashing = false;
        DashIndicator.enabled = true;
    }


    private void OnEnable()
    {
        DashIndicator.enabled = true;
    }

    private void OnDisable()
    {
        DashIndicator.enabled = false;
    }

}
