using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public float checkDelay = 0.5f;
    //Should have a list of enemy GameObjects to check for death.
    public int enemiesInArena = 0;
    //Should have a list of doors to activate and deactivate.
    public GameObject[] doors;
    //A box collider to activate on the player's first entrance.
    public BoxCollider trigger;

    public bool debugExtents = false;

    Vector3 triggerSize => new Vector3(trigger.size.x * transform.localScale.x, trigger.size.y * transform.localScale.y, trigger.size.z * transform.localScale.z);

    private void OnDrawGizmos()
    {
        if (debugExtents)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, triggerSize);
        }
    }

    private void Start()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, triggerSize, trigger.transform.rotation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemiesInArena++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Arena activating");
            if (enemiesInArena > 0)
            {
                Activate();
            }
        }
    }

    void Activate()
    {
        foreach (GameObject door in doors)
        {
            //Get the door script and call the activation function.
            Door doorScript = door.GetComponent<Door>();
            if (doorScript)
                doorScript.Activate();
        }
    }

    void Deactivate()
    {

        foreach (GameObject door in doors)
        {
            //Get the door script and call the deactivation function.
            Door doorScript = door.GetComponent<Door>();
            if (doorScript)
                doorScript.Deactivate();
        }
    }

    public void DecrementEnemyCounter()
    {
        enemiesInArena--;
        if (enemiesInArena == 0)
        {
            print("Deactivating");
            Deactivate();
        }
    }
}
