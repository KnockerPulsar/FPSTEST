using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for pushing the player when they overlap the ramp hitbox.
public class RampScript : MonoBehaviour
{
    public PlayerVariables pVars;               //A container responsible for storing commonly used / shared variables.
    Renderer rampRenderer;                      //The ramp's renderer, used to reference its material to scroll it.
    Vector2 matOffsets = new Vector2(0,0);      //The material offsets.
    public float rampMultiplier = 1000f;        //The ramp force multiplier.
    public float upMultiplier = 4f;             //The upwards force multiplier.
    public float forwardMultiplier = 5f;        //The forwards force multiplier.

    // Start is called before the first frame update
    void Start()
    {
        rampRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    //Updates the ramp's material to give the illusion of scrolling.
    void Update()
    {
        matOffsets.x -= 0.01f;
        rampRenderer.material.SetTextureOffset("_MainTex", matOffsets);
    }


    //Whenever the player overlaps the ramp hitbox, applies a force forwards and upwards.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hit ramp!");
            pVars.playerRB.AddForce(Vector3.up * rampMultiplier * Time.deltaTime * pVars.playerRB.mass * upMultiplier);
            pVars.playerRB.AddForce(transform.right * rampMultiplier * Time.deltaTime * pVars.playerRB.mass * forwardMultiplier);
        }
    }
}
