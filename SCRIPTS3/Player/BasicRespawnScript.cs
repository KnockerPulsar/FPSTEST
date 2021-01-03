using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Responsible for returning the player to the initial spawn point the X is pressed.
//Also quits the game on Escape.
public class BasicRespawnScript : MonoBehaviour
{
      //A container responsible for storing commonly used / shared variables.
    Vector3 spawnPosition;        //Stores the initial spawn position for the player.

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = gameObject.transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        //Resets the player's time, position, rotation and velocity when X is pressed.
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerVariables.secondsSinceLevelStart = 0;
            PlayerVariables.playerRB.gameObject.transform.position = spawnPosition;
            PlayerVariables.playerRB.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            PlayerVariables.playerRB.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
        //Quits the game if the player presses Escape and isn't in the editor.
        else if (Input.GetKeyDown(KeyCode.Escape) && !Application.isEditor)
            Application.Quit();
    }
}
