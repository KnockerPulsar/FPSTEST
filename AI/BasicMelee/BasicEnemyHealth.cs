﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A basic script used manage the health of enemies and their death.
public class BasicEnemyHealth : EnemyHealth
{
    public ParticleSystem DeathPS;              //The particle effect played on death.
    public GameObject body;                     //The body of the enemy, it's de-activated on death to allow for the particle effect to play.
    AudioSource source;                         //The audio source responsible for playing the death sound.
    float deleteDelay = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        DeathPS.Stop();
    }

    //Hides the enemy's body, plays the death sound and particle system, then deletes the enemy after they finish.
    public override void OnDeath()
    {
        body.SetActive(false);
        source.Play();
        DeathPS.Play();
        GetComponent<BaseAIStateMachine>().enabled = false;
        if (pVars.playerHealth <= healthSpawnThreshold)
        {
            pickupSpawnerObj.SetActive(true);
            SpawnHealth();
            TimedDestroy TD = pickupSpawnerScript.pickup.GetComponent<TimedDestroy>();
            deleteDelay = pickupSpawnerScript.spawnCount * pickupSpawnerScript.spawnDelay + TD.time;
        }
        else
            deleteDelay = source.clip.length;
        MassegeArena();
        Invoke(nameof(EndDeath), deleteDelay);
    }

    void EndDeath()
    {
        source.Stop();
        DeathPS.Stop();
        Destroy(gameObject);
    }


    void SpawnHealth()
    {
        pickupSpawnerScript = pickupSpawnerObj.GetComponent<PickupSpawner>();
        if (pickupSpawnerScript && deathPickups.Length > 0)
        {
            pickupSpawnerScript.pickup = deathPickups[0];
            pickupSpawnerScript.spawnCount = Mathf.RoundToInt((pVars.playerMaxHealth - pVars.playerHealth) / 40);
            pickupSpawnerScript.enabled = true;
        }
    }

    void MassegeArena()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one);
        foreach (Collider col in colliders)
        {
            Arena arena = col.GetComponent<Arena>();
            if (arena)
            {
                print("Arena found");
                arena.DecrementEnemyCounter();
            }
        }
    }
}
