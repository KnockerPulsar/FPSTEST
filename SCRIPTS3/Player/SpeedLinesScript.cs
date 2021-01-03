using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for providing visual feedback through the speedlines
public class SpeedLinesScript : MonoBehaviour
{
                 //A container responsible for storing commonly used / shared variables.
    public ParticleSystem speedLines;        //The speedlines game object.
    public float multiplier = 1.5f;          //Used to calculate the threshold velocity
    float thresholdVelocity;          //The threshold velocity after which the speedlines appear.

    // Start is called before the first frame update
    //Finds the speedlines if they're not input in the inspector and calculates the threshold velocity.
    //Hides the speedlines.
    void Start()
    {
        if (!speedLines)
            speedLines = GameObject.Find("Speed Lines").GetComponent<ParticleSystem>();
        thresholdVelocity = PlayerVariables.maxVelocity * multiplier;
        speedLines.Stop();
    }

    // Update is called once per frame
    //Checks if the player's velocity > the threshold velocity and if so, it shows the speed lines, otherwise, hides them.
    void Update()
    {
        if (PlayerVariables.playerRB.velocity.magnitude > thresholdVelocity)
            speedLines.Play();
        else
            speedLines.Stop();
        
    }
}
