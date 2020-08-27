using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

//Responsible for spawning a jump pad below the object this script is on whenever the player first collides with this object.
//Used as a checkpoint in the final tutorial room.
public class JumpPadSpawnerScript : MonoBehaviour
{
    bool active = false;            //Used to check if the player already collided with this object already.
    public GameObject JumpPad;      //The jump pad prefab.
    JumpPadScript jpScript;         //The spawned prefab's script.

    //If the player overlaps the object's hitbox, checks if it's the first overlap and if so, does a raycast and spawns a jump pad
    //where it hit, set's the multiplier to the distance between the spawning object and the spawned object * 2. (tuned via testing).
    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            active = true;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up);
            if (hits.Length > 0)
            {
                Debug.Log("JUMPPAD SPAWNED");
                jpScript = Instantiate(JumpPad, hits[0].point, Quaternion.identity).GetComponent<JumpPadScript>();
                jpScript.jumpForceMultiplier = Vector3.Distance(hits[0].point, transform.position) * 2f;
            }
        }
    }
}
