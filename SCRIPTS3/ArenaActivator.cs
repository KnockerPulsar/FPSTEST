using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaActivator : MonoBehaviour
{
    public Arena arenaScirpt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(arenaScirpt.enemiesInArena > 0 && !arenaScirpt.activated)
            {
                StartCoroutine(arenaScirpt.Activate());
            }
        }
    }
}
