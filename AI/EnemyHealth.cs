using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages each enemy's health.
//All functions left as virtual to enable different behaviour whenever needed.
public class EnemyHealth : MonoBehaviour
{
    public PlayerVariables pVars;
    public float currentHealth = 100f;              //The current health the enemy has.
    public float maxHealth = 100f;                  //The maximum health the enemy can reach.
    public float healthSpawnThreshold = 30f;
    public GameObject[] deathPickups;
    public GameObject pickupSpawnerObj; 
    protected PickupSpawner pickupSpawnerScript;

    //Should be called each time the enemy recieves damage.
    public virtual void RecieveDamage(float amount)
    {
        if (amount >= currentHealth && currentHealth > 0)
        {
            currentHealth = 0;
            OnDeath();
        }
        else
            currentHealth -= amount;
    }

    //Should be called each time the enemy heals.
    public virtual void Heal(float amount)
    {
        if (amount + currentHealth > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
    }

    //Should be called when the death condition occurs ( health <= 0 generally).
    public virtual void OnDeath()
    {
    }
}
