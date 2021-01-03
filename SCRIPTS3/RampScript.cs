using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for pushing the player when they overlap the ramp hitbox.
public class RampScript : MonoBehaviour
{
    Renderer rampRenderer;                      //The ramp's renderer, used to reference its material to scroll it.
    Vector2 matOffsets = new Vector2(0, 0);      //The material offsets.
    public float rampMultiplier = 1000f;        //The ramp force multiplier.
    public float upMultiplier = 4f;             //The upwards force multiplier.
    public float forwardMultiplier = 5f;        //The forwards force multiplier.

    //Whenever the player overlaps the ramp hitbox, applies a force forwards and upwards.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Hit ramp!");
            PlayerVariables.playerRB.velocity = Vector3.zero;
            PlayerVariables.playerRB.velocity += (Vector3.up * rampMultiplier * PlayerVariables.playerRB.mass * upMultiplier);
            PlayerVariables.playerRB.velocity += (transform.right * rampMultiplier * PlayerVariables.playerRB.mass * forwardMultiplier);
        }
    }
}
