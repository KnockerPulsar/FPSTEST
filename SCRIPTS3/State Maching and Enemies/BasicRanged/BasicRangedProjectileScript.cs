using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedProjectileScript : MonoBehaviour
{
    public float ForceMultiplier = 10000f;
    public float damage = 10f;
    Rigidbody RB;

    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        RB.AddForce(gameObject.transform.forward     * ForceMultiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivator") || other.CompareTag("Projectile") || other.CompareTag("Enemy") || other.CompareTag("Arena"))
        {
            //print("Hit " + other.tag);
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (other.isTrigger == true)
                return;

            //print("Projectile hit player");
            Invoke(nameof(DelayedDestroy), 0.1f);
            PlayerHealthManager playerHM = other.gameObject.GetComponent<PlayerHealthManager>();
            if (playerHM)
                playerHM.RecieveDamage(damage);
        }
        else
        {
            //print("Destroying projectile, hit " + other.name);
            Destroy(gameObject);
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
